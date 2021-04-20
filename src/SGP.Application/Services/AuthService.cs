using Ardalis.GuardClauses;
using FluentResults;
using Microsoft.Extensions.Options;
using SGP.Application.Interfaces;
using SGP.Application.Requests.AuthRequests;
using SGP.Application.Responses;
using SGP.Domain.Entities.UsuarioAggregate;
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
    public class AuthService : IAuthService
    {
        private readonly AuthConfig _authConfig;
        private readonly IDateTime _dateTime;
        private readonly IHashService _hashService;
        private readonly IUsuarioRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly ITokenClaimsService _tokenClaimsService;

        public AuthService
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

        public async Task<Result<TokenResponse>> AuthenticateAsync(AuthRequest request)
        {
            // Validando a requisição.
            var result = await new AuthRequestValidator().ValidateAsync(request);
            if (!result.IsValid)
            {
                // Retornando os erros.
                return result.ToFail<TokenResponse>();
            }

            // Criando o Objeto de Valor (VO).
            var email = new Email(request.Email);

            // Obtendo o usuário pelo e-mail.
            var usuario = await _repository.GetByEmailAsync(email);
            if (usuario == null)
            {
                return Result.Fail<TokenResponse>("A conta informada não existe.");
            }

            // Verificando se a conta está bloqueada.
            if (usuario.ContaEstaBloqueada(_dateTime))
            {
                return Result.Fail<TokenResponse>("A sua conta está bloqueada, entre em contato com o nosso suporte.");
            }

            // Verificando se a senha corresponde a senha gravada na base de dados.
            if (_hashService.Compare(request.Senha, usuario.Senha))
            {
                var claims = GenerateClaims(usuario);
                var accessToken = _tokenClaimsService.GenerateAccessToken(claims);
                var refreshToken = _tokenClaimsService.GenerateRefreshToken();

                usuario.IncrementarAcessoComSucceso();
                usuario.AdicionarRefreshToken(new RefreshToken(
                    refreshToken.Token,
                    accessToken.CreatedAt,
                    accessToken.ExpiresAt));

                _repository.Update(usuario);
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
                usuario.IncrementarAcessoComFalha(
                    _dateTime,
                    _authConfig.MaximumAttempts,
                    _authConfig.SecondsBlocked);

                _repository.Update(usuario);
                await _uow.SaveChangesAsync();

                return Result.Fail<TokenResponse>("O e-mail ou senha está incorreta.");
            }
        }

        public async Task<Result<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            // Validando a requisição.
            var result = await new RefreshTokenRequestValidator().ValidateAsync(request);
            if (!result.IsValid)
            {
                // Retornando os erros.
                return result.ToFail<TokenResponse>();
            }

            // Obtendo o usuário e seus tokens pelo RefreshToken.
            var usuario = await _repository.GetByTokenAsync(request.Token);
            if (usuario == null)
            {
                return Result.Fail<TokenResponse>("Nenhum token encontrado.");
            }

            var refreshToken = usuario.RefreshTokens.FirstOrDefault(t => t.Token == request.Token);
            if (refreshToken.IsExpired(_dateTime))
            {
                return Result.Fail<TokenResponse>("O token inválido ou expirado.");
            }

            var claims = GenerateClaims(usuario);
            var accessToken = _tokenClaimsService.GenerateAccessToken(claims);
            var newRefreshToken = _tokenClaimsService.GenerateRefreshToken();

            // Substituindo o token de atualização atual por um novo.
            refreshToken.Revoke(newRefreshToken.Token, accessToken.CreatedAt);

            // Adicionando o novo.
            usuario.AdicionarRefreshToken(new RefreshToken(
                newRefreshToken.Token,
                accessToken.CreatedAt,
                accessToken.ExpiresAt));

            _repository.Update(usuario);
            await _uow.SaveChangesAsync();

            return Result.Ok(new TokenResponse(
                true,
                accessToken.Token,
                accessToken.CreatedAt,
                accessToken.ExpiresAt,
                newRefreshToken.Token));
        }

        private static IEnumerable<Claim> GenerateClaims(Usuario usuario)
        {
            return new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Nome),
                new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Id.ToString())
            };
        }
    }
}