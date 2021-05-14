using Ardalis.GuardClauses;
using FluentResults;
using Microsoft.Extensions.Options;
using SGP.Application.Interfaces;
using SGP.Application.Requests.AuthRequests;
using SGP.Application.Responses;
using SGP.Domain.Entities.UserAggregate;
using SGP.Domain.Repositories;
using SGP.Domain.ValueObjects;
using SGP.Shared.AppSettings;
using SGP.Shared.Errors;
using SGP.Shared.Extensions;
using SGP.Shared.Interfaces;
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
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly ITokenClaimsService _tokenClaimsService;

        public AuthService
        (
            IOptions<AuthConfig> authOptions,
            IDateTime dateTime,
            IHashService hashService,
            ITokenClaimsService tokenClaimsService,
            IUserRepository repository,
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

        public async Task<Result<TokenResponse>> AuthenticateAsync(AuthRequest request)
        {
            var result = await new AuthRequestValidator().ValidateAsync(request);
            if (!result.IsValid)
            {
                return result.ToFail<TokenResponse>();
            }

            var user = await _repository.GetByEmailAsync(new Email(request.Email));
            if (user == null)
            {
                return Result.Fail<TokenResponse>(new NotFoundError("A conta informada não existe."));
            }

            // Verificando se a conta está bloqueada.
            if (user.IsLocked(_dateTime))
            {
                return Result.Fail<TokenResponse>("A sua conta está bloqueada, entre em contato com o nosso suporte.");
            }

            // Verificando se a senha corresponde a senha criptografada gravada na base de dados.
            if (_hashService.Compare(request.Password, user.PasswordHash))
            {
                // Gerando as regras (ROLES).
                var claims = GenerateClaims(user);

                // Gerando o TOKEN de acesso.
                var accessToken = _tokenClaimsService.GenerateAccessToken(claims);

                // Gerando o TOKEN de atualização.
                var refreshToken = _tokenClaimsService.GenerateRefreshToken();

                // Adicionando o TOKEN atualização.
                user.AddRefreshToken(new RefreshToken(
                    refreshToken,
                    accessToken.CreatedAt,
                    accessToken.ExpiresAt));

                // Atualizando no repositório do usuário.
                _repository.Update(user);

                // Confirmando a transação.
                await _uow.SaveChangesAsync();

                return Result.Ok(new TokenResponse(
                    true,
                    accessToken.Token,
                    accessToken.CreatedAt,
                    accessToken.ExpiresAt,
                    refreshToken));
            }
            else
            {
                // Se o LOGIN for inválido, será incrementado o número de falhas,
                // se atingido o limite de tentativas de acesso a conta será bloqueada por um determinado tempo.
                var lockedTimeSpan = TimeSpan.FromSeconds(_authConfig.SecondsBlocked);
                user.IncrementFailuresNum(_dateTime, _authConfig.MaximumAttempts, lockedTimeSpan);

                // Atualizando no repositório do usuário.
                _repository.Update(user);

                // Confirmando a transação.
                await _uow.SaveChangesAsync();

                return Result.Fail<TokenResponse>("O e-mail ou senha está incorreta.");
            }
        }

        public async Task<Result<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var result = await new RefreshTokenRequestValidator().ValidateAsync(request);
            if (!result.IsValid)
            {
                return result.ToFail<TokenResponse>();
            }

            var user = await _repository.GetByTokenAsync(request.Token);
            if (user == null)
            {
                return Result.Fail<TokenResponse>(new NotFoundError("Nenhum token encontrado."));
            }

            // Verificando se o TOKEN de atualização está expirado.
            var refreshToken = user.RefreshTokens.FirstOrDefault(t => t.Token == request.Token);
            if (refreshToken.IsExpired(_dateTime))
            {
                return Result.Fail<TokenResponse>("O token inválido ou expirado.");
            }

            // Gerando as regras (ROLES).
            var claims = GenerateClaims(user);

            // Gerando o TOKEN de acesso.
            var accessToken = _tokenClaimsService.GenerateAccessToken(claims);

            // Gerando o TOKEN de atualização.
            var newRefreshToken = _tokenClaimsService.GenerateRefreshToken();

            // Substituindo o TOKEN de atualização atual pelo o novo.
            refreshToken.Revoke(newRefreshToken, accessToken.CreatedAt);

            // Adicionando o novo TOKEN atualização.
            user.AddRefreshToken(new RefreshToken(
                newRefreshToken,
                accessToken.CreatedAt,
                accessToken.ExpiresAt));

            // Atualizando no repositório do usuário.
            _repository.Update(user);

            // Confirmando a transação.
            await _uow.SaveChangesAsync();

            return Result.Ok(new TokenResponse(
                true,
                accessToken.Token,
                accessToken.CreatedAt,
                accessToken.ExpiresAt,
                newRefreshToken));
        }

        private static IEnumerable<Claim> GenerateClaims(User user)
        {
            return new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Name, ClaimValueTypes.String),
                new Claim(JwtRegisteredClaimNames.Email, user.Email.ToString(), ClaimValueTypes.Email)
            };
        }
    }
}