using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SGP.Application.Interfaces;
using SGP.Application.Requests.AuthRequests;
using SGP.Application.Responses;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Domain.ValueObjects;
using SGP.Shared.AppSettings;
using SGP.Shared.Extensions;
using SGP.Shared.Interfaces;
using SGP.Shared.Results;
using SGP.Shared.UnitOfWork;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SGP.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthConfig _authConfig;
        private readonly JwtConfig _jwtConfig;
        private readonly IDateTimeService _dateTimeService;
        private readonly IHashService _hashService;
        private readonly IUsuarioRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<AuthService> _logger;

        public AuthService
        (
            IOptions<AuthConfig> authOptions,
            IOptions<JwtConfig> jwtOptions,
            IDateTimeService dateTimeService,
            IHashService hashService,
            IUsuarioRepository repository,
            IUnitOfWork uow,
            ILogger<AuthService> logger
        )
        {
            _authConfig = authOptions?.Value ?? throw new ArgumentNullException(nameof(authOptions),
                $"A seção '{nameof(AuthConfig)}' não está configurada no appsettings.json");

            _jwtConfig = jwtOptions?.Value ?? throw new ArgumentNullException(nameof(jwtOptions),
                $"A seção '{nameof(JwtConfig)}' não está configurada no appsettings.json");

            _dateTimeService = dateTimeService;
            _hashService = hashService;
            _repository = repository;
            _uow = uow;
            _logger = logger;
        }

        public async Task<IResult<TokenResponse>> AuthenticateAsync(AuthRequest request)
        {
            var result = new Result<TokenResponse>();

            // Validando a requisição.
            request.Validate();
            if (!request.IsValid)
            {
                // Retornando os erros.
                return result.Fail(request.Notifications);
            }

            // Obtendo o usuário pelo e-mail.
            var usuario = await _repository.GetByEmailAsync(new Email(request.Email));
            if (usuario == null)
            {
                return result.Fail("A conta informada não existe.");
            }

            // Verificando se a conta está bloqueada.
            if (usuario.ContaEstaBloqueada(_dateTimeService))
            {
                return result.Fail("A sua conta está bloqueada, entre em contato com o nosso suporte.");
            }

            // Verificando se a senha corresponde a senha gravada na base de dados.
            if (_hashService.Compare(request.Senha, usuario.Senha))
            {
                var secondsToExpire = _jwtConfig.Seconds;
                var createdAt = _dateTimeService.Now;
                var expiresAt = createdAt.AddSeconds(secondsToExpire);
                var refreshToken = GenerateRefreshToken();

                usuario.IncrementarAcessoComSucceso();
                usuario.AdicionarRefreshToken(new RefreshToken(refreshToken, createdAt, expiresAt));

                if (!usuario.IsValid)
                {
                    usuario.ToLog(_logger);
                    return result.Fail("Não foi possível efetuar a autorização, tente novamente.");
                }

                _repository.Update(usuario);
                await _uow.SaveChangesAsync();

                var token = new TokenResponse(
                    true,
                    GenerateJwtToken(usuario, createdAt, expiresAt),
                    createdAt,
                    expiresAt,
                    refreshToken,
                    secondsToExpire);

                return result.Success(token, "Autenticado efetuada com sucesso.");
            }
            else
            {
                usuario.IncrementarAcessoComFalha(
                    _dateTimeService,
                    _authConfig.MaximumAttempts,
                    _authConfig.SecondsBlocked);

                _repository.Update(usuario);
                await _uow.SaveChangesAsync();

                return result.Fail("O e-mail ou senha está incorreta.");
            }
        }

        public async Task<IResult<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var result = new Result<TokenResponse>();

            // Validando a requisição.
            request.Validate();
            if (!request.IsValid)
            {
                // Retornando os erros.
                return result.Fail(request.Notifications);
            }

            // Obtendo o usuário e seus tokens pelo RefreshToken.
            var usuario = await _repository.GetByTokenAsync(request.Token);
            if (usuario == null)
            {
                return result.Fail("Nenhum token encontrado.");
            }

            var refreshToken = usuario.RefreshTokens.FirstOrDefault(t => t.Token == request.Token);
            if (refreshToken.IsExpired(_dateTimeService))
            {
                return result.Fail("O token inválido ou expirado.");
            }

            var secondsToExpire = _jwtConfig.Seconds;
            var createdAt = _dateTimeService.Now;
            var expiresAt = createdAt.AddSeconds(secondsToExpire);
            var newRefreshToken = GenerateRefreshToken();

            // Substituindo o token de atualização atual por um novo.
            refreshToken.Revoke(newRefreshToken, _dateTimeService.Now);

            // Adicionando o novo.
            usuario.AdicionarRefreshToken(new RefreshToken(newRefreshToken, createdAt, expiresAt));

            if (!usuario.IsValid)
            {
                usuario.ToLog(_logger);
                return result.Fail("Não foi possível efetuar a atualização do acesso, tente novamente.");
            }

            _repository.Update(usuario);
            await _uow.SaveChangesAsync();

            var token = new TokenResponse(
                true,
                GenerateJwtToken(usuario, createdAt, expiresAt),
                createdAt,
                expiresAt,
                newRefreshToken,
                secondsToExpire);

            return result.Success(token, "Atualização efetuada com sucesso.");
        }

        private string GenerateJwtToken(Usuario usuario, DateTime createdAt, DateTime expiresAt)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Nome),
                new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims);

            var secretKey = Encoding.UTF8.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtConfig.Audience,
                Issuer = _jwtConfig.Issuer,
                Subject = identity,
                NotBefore = createdAt,
                Expires = expiresAt,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static string GenerateRefreshToken()
        {
            using (var cryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                cryptoServiceProvider.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes).RemoveIlegalCharactersFromURL();
            }
        }
    }
}