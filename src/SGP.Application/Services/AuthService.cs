using Ardalis.GuardClauses;
using Microsoft.Extensions.Options;
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
using System.Threading.Tasks;

namespace SGP.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthConfig _authConfig;
        private readonly IDateTime _dateTime;
        private readonly IHashService _hashService;
        private readonly IJWTokenService _tokenService;
        private readonly IUsuarioRepository _repository;
        private readonly IUnitOfWork _uow;

        public AuthService
        (
            IOptions<AuthConfig> authOptions,
            IDateTime dateTime,
            IHashService hashService,
            IJWTokenService tokenService,
            IUsuarioRepository repository,
            IUnitOfWork uow
        )
        {
            Guard.Against.Null(authOptions);

            _authConfig = authOptions.Value;
            _dateTime = dateTime;
            _hashService = hashService;
            _tokenService = tokenService;
            _repository = repository;
            _uow = uow;
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
            if (usuario.ContaEstaBloqueada(_dateTime))
            {
                return result.Fail("A sua conta está bloqueada, entre em contato com o nosso suporte.");
            }

            // Verificando se a senha corresponde a senha gravada na base de dados.
            if (_hashService.Compare(request.Senha, usuario.Senha))
            {
                // Gerando Json Web Token
                var token = _tokenService.GenerateToken(GenerateClaims(usuario));

                usuario.AdicionarRefreshToken(new RefreshToken(token.RefreshToken, token.CreatedAt, token.ExpiresAt));
                usuario.IncrementarAcessoComSucceso();

                _repository.Update(usuario);
                await _uow.SaveChangesAsync();

                return result.Success(new TokenResponse(
                    true,
                    token.AccessToken,
                    token.CreatedAt,
                    token.ExpiresAt,
                    token.RefreshToken,
                    token.SecondsToExpire), "Autenticado efetuada com sucesso.");
            }
            else
            {
                usuario.IncrementarAcessoComFalha(
                    _dateTime,
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
            if (refreshToken.IsExpired(_dateTime))
            {
                return result.Fail("O token inválido ou expirado.");
            }

            // Gerando Json Web Token
            var token = _tokenService.GenerateToken(GenerateClaims(usuario));

            // Substituindo o token de atualização atual por um novo.
            refreshToken.Revoke(token.RefreshToken, _dateTime.Now);

            // Adicionando o novo.
            usuario.AdicionarRefreshToken(new RefreshToken(token.RefreshToken, token.CreatedAt, token.ExpiresAt));

            _repository.Update(usuario);
            await _uow.SaveChangesAsync();

            return result.Success(new TokenResponse(
                true,
                token.AccessToken,
                token.CreatedAt,
                token.ExpiresAt,
                token.RefreshToken,
                token.SecondsToExpire), "Atualização efetuada com sucesso.");
        }

        private static IEnumerable<Claim> GenerateClaims(Usuario usuario)
        {
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Nome),
                new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Id.ToString())
            };
        }
    }
}