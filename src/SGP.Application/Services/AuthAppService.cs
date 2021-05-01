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
using SGP.Shared.Extensions;
using SGP.Shared.Interfaces;
using SGP.Shared.UnitOfWork;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SGP.Application.Services
{
    public class AuthAppService : IAuthAppService
    {
        private readonly AuthConfig _authConfig;
        private readonly IDateTime _dateTime;
        private readonly IHashService _hashService;
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly ITokenClaimsService _tokenClaimsService;

        public AuthAppService
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
                return Result.Fail<TokenResponse>("A conta informada não existe.");
            }

            if (user.IsLocked(_dateTime))
            {
                return Result.Fail<TokenResponse>("A sua conta está bloqueada, entre em contato com o nosso suporte.");
            }

            if (_hashService.Compare(request.Password, user.PasswordHash))
            {
                var claims = GenerateClaims(user);
                var accessToken = _tokenClaimsService.GenerateAccessToken(claims);
                var refreshToken = _tokenClaimsService.GenerateRefreshToken();

                user.AddRefreshToken(new RefreshToken(
                    refreshToken.Token,
                    accessToken.CreatedAt,
                    accessToken.ExpiresAt));

                _repository.Update(user);
                await _uow.SaveChangesAsync();

                return Result.Ok(new TokenResponse(
                    true,
                    accessToken.Token,
                    accessToken.CreatedAt,
                    accessToken.ExpiresAt,
                    refreshToken.Token));
            }
            else
            {
                user.IncrementFailuresNum(
                    _dateTime,
                    _authConfig.MaximumAttempts,
                    _authConfig.SecondsBlocked);

                _repository.Update(user);
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
                return Result.Fail<TokenResponse>("Nenhum token encontrado.");
            }

            var refreshToken = user.RefreshTokens.FirstOrDefault(t => t.Token == request.Token);
            if (refreshToken.IsExpired(_dateTime))
            {
                return Result.Fail<TokenResponse>("O token inválido ou expirado.");
            }

            var claims = GenerateClaims(user);
            var accessToken = _tokenClaimsService.GenerateAccessToken(claims);
            var newRefreshToken = _tokenClaimsService.GenerateRefreshToken();

            refreshToken.Revoke(newRefreshToken.Token, accessToken.CreatedAt);

            user.AddRefreshToken(new RefreshToken(
                newRefreshToken.Token,
                accessToken.CreatedAt,
                accessToken.ExpiresAt));

            _repository.Update(user);
            await _uow.SaveChangesAsync();

            return Result.Ok(new TokenResponse(
                true,
                accessToken.Token,
                accessToken.CreatedAt,
                accessToken.ExpiresAt,
                newRefreshToken.Token));
        }

        private static IEnumerable<Claim> GenerateClaims(User user)
        {
            return new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString())
            };
        }
    }
}