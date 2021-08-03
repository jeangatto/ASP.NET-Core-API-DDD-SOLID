using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using FluentResults;
using Microsoft.Extensions.Options;
using SGP.Application.Interfaces;
using SGP.Application.Requests.AuthRequests;
using SGP.Application.Responses;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Domain.ValueObjects;
using SGP.Shared.AppSettings;
using SGP.Shared.Errors;
using SGP.Shared.Extensions;
using SGP.Shared.Interfaces;

namespace SGP.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Fields

        private readonly AuthConfig _authConfig;
        private readonly IDateTime _dateTime;
        private readonly IHashService _hashService;
        private readonly IUsuarioRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly ITokenClaimsService _tokenClaimsService;

        #endregion

        #region Constructor

        public AuthenticationService
        (
            IOptions<AuthConfig> authOptions,
            IDateTime dateTime,
            IHashService hashService,
            ITokenClaimsService tokenClaimsService,
            IUsuarioRepository repository,
            IUnitOfWork uow
        )
        {
            Guard.Against.Null(authOptions, nameof(authOptions));

            _authConfig = authOptions.Value;
            _dateTime = dateTime;
            _hashService = hashService;
            _tokenClaimsService = tokenClaimsService;
            _repository = repository;
            _uow = uow;
        }

        #endregion

        #region Methods

        public async Task<Result<TokenResponse>> AuthenticateAsync(AuthRequest request)
        {
            // Validando a requisição.
            request.Validate();
            if (!request.IsValid)
            {
                // Retornando os erros da validação.
                return request.ToFail<TokenResponse>();
            }

            var usuario = await _repository.ObterPorEmailAsync(new Email(request.Email));
            if (usuario == null)
            {
                return Result.Fail<TokenResponse>(new NotFoundError("A conta informada não existe."));
            }

            // Verificando se a conta está bloqueada.
            if (usuario.EstaBloqueado(_dateTime))
            {
                return Result.Fail<TokenResponse>("A sua conta está bloqueada, entre em contato com o nosso suporte.");
            }

            // Verificando se a senha corresponde a senha criptografada gravada na base de dados.
            if (_hashService.Compare(request.Password, usuario.HashSenha))
            {
                // Gerando as regras (roles).
                var claims = GenerateClaims(usuario);

                // Gerando o token de acesso.
                var accessToken = _tokenClaimsService.GenerateAccessToken(claims);

                // Gerando o token de atualização.
                var refreshToken = _tokenClaimsService.GenerateRefreshToken();

                // Vinculando o token atualização ao usuário.
                usuario.AdicionarToken(new TokenAcesso(
                    refreshToken,
                    accessToken.CreatedAt,
                    accessToken.ExpiresAt));

                _repository.Update(usuario);
                await _uow.SaveChangesAsync();

                return Result.Ok(new TokenResponse(
                    accessToken.Token,
                    accessToken.CreatedAt,
                    accessToken.ExpiresAt,
                    refreshToken));
            }
            else
            {
                // Se o login for inválido, será incrementado o número de falhas,
                // se atingido o limite de tentativas de acesso a conta será bloqueada por um determinado tempo.
                var lockedTimeSpan = TimeSpan.FromSeconds(_authConfig.SecondsBlocked);
                usuario.IncrementarFalhas(_dateTime, _authConfig.MaximumAttempts, lockedTimeSpan);

                _repository.Update(usuario);
                await _uow.SaveChangesAsync();

                return Result.Fail<TokenResponse>("O e-mail ou senha está incorreta.");
            }
        }

        public async Task<Result<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            // Validando a requisição.
            request.Validate();
            if (!request.IsValid)
            {
                // Retornando os erros da validação.
                return request.ToFail<TokenResponse>();
            }

            var usuario = await _repository.ObterPorTokenAsync(request.Token);
            if (usuario == null)
            {
                return Result.Fail<TokenResponse>(new NotFoundError("Nenhum token encontrado."));
            }

            // Verificando se o token de atualização está expirado.
            var tokenAcesso = usuario.Tokens.FirstOrDefault(t => t.Token == request.Token);
            if (tokenAcesso == null || !tokenAcesso.EstaValido(_dateTime))
            {
                return Result.Fail<TokenResponse>("O token inválido ou expirado.");
            }

            // Gerando as regras (roles).
            var claims = GenerateClaims(usuario);

            // Gerando um novo token de acesso.
            var accessToken = _tokenClaimsService.GenerateAccessToken(claims);

            // Gerando um novo token de atualização.
            var newRefreshToken = _tokenClaimsService.GenerateRefreshToken();

            // Revogando (cancelando) o token de atualização atual.
            tokenAcesso.RevogarToken(accessToken.CreatedAt);

            // Vinculando o novo token atualização ao usuário.
            usuario.AdicionarToken(new TokenAcesso(
                newRefreshToken,
                accessToken.CreatedAt,
                accessToken.ExpiresAt));

            _repository.Update(usuario);
            await _uow.SaveChangesAsync();

            return Result.Ok(new TokenResponse(
                accessToken.Token,
                accessToken.CreatedAt,
                accessToken.ExpiresAt,
                newRefreshToken));
        }

        private static Claim[] GenerateClaims(Usuario usuario)
        {
            return new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Nome, ClaimValueTypes.String),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email.ToString(), ClaimValueTypes.Email)
            };
        }

        #endregion
    }
}