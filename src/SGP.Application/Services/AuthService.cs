using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SGP.Application.Interfaces;
using SGP.Application.Requests.AuthRequests;
using SGP.Application.Responses;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Domain.ValueObjects;
using SGP.Shared.AppSettings;
using SGP.Shared.Interfaces;
using SGP.Shared.Results;
using SGP.Shared.UnitOfWork;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SGP.Application.Services
{
    public class AuthService : IAuthService
    {
        #region Constructor

        private readonly AuthConfig _authConfig;
        private readonly IDateTimeService _dateTimeService;
        private readonly IHashService _hashService;
        private readonly JwtConfig _jwtConfig;
        private readonly IUsuarioRepository _repository;
        private readonly IUnitOfWork _uow;

        public AuthService
        (
            IOptions<AuthConfig> authOptions,
            IOptions<JwtConfig> jwtOptions,
            IDateTimeService dateTimeService,
            IHashService hashService,
            IUsuarioRepository repository,
            IUnitOfWork uow
        )
        {
            _authConfig = authOptions?.Value ?? throw new ArgumentNullException(nameof(authOptions), $"A seção '{nameof(AuthConfig)}' não está configurada no appsettings.json");
            _jwtConfig = jwtOptions?.Value ?? throw new ArgumentNullException(nameof(jwtOptions), $"A seção '{nameof(JwtConfig)}' não está configurada no appsettings.json");
            _dateTimeService = dateTimeService;
            _hashService = hashService;
            _repository = repository;
            _uow = uow;
        }

        #endregion

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
                var createdAt = _dateTimeService.Now;
                var expiresAt = createdAt.AddSeconds(_jwtConfig.Seconds);
                var accessToken = GenerateJwtToken(usuario, createdAt, expiresAt);
                var refreshToken = GenerateRefreshToken();

                usuario.IncrementarAcessoComSucceso();
                usuario.AdicionarToken(new UsuarioToken(refreshToken, createdAt, expiresAt));
                _repository.Update(usuario);
                await _uow.SaveChangesAsync();

                var token = new TokenResponse(
                    true,
                    accessToken,
                    createdAt,
                    expiresAt,
                    refreshToken,
                    _jwtConfig.Seconds);

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

            throw new NotImplementedException();
        }

        private static string GenerateRefreshToken()
        {
            using (var cryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                cryptoServiceProvider.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
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

            var tokenHandler = new JwtSecurityTokenHandler();

            var secretKey = Encoding.UTF8.GetBytes(_jwtConfig.Secret);

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtConfig.Audience,
                Issuer = _jwtConfig.Issuer,
                SigningCredentials = signingCredentials,
                Subject = identity,
                NotBefore = createdAt,
                Expires = expiresAt
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}