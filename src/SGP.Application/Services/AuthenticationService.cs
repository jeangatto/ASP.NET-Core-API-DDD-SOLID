using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentResults;
using Microsoft.Extensions.Options;
using SGP.Application.Interfaces;
using SGP.Application.Requests.AuthenticationRequests;
using SGP.Application.Responses;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Domain.ValueObjects;
using SGP.Shared.AppSettings;
using SGP.Shared.Errors;
using SGP.Shared.Extensions;
using SGP.Shared.Interfaces;

namespace SGP.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    #region Fields

    private readonly AuthConfig _authConfig;
    private readonly IDateTime _dateTime;
    private readonly IHashService _hashService;
    private readonly ITokenClaimsService _tokenClaimsService;
    private readonly IUsuarioRepository _repository;
    private readonly IUnitOfWork _uow;

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
        _authConfig = authOptions.Value;
        _dateTime = dateTime;
        _hashService = hashService;
        _tokenClaimsService = tokenClaimsService;
        _repository = repository;
        _uow = uow;
    }

    #endregion

    #region Methods

    public async Task<Result<TokenResponse>> AuthenticateAsync(LogInRequest request)
    {
        // Validando a requisição.
        await request.ValidateAsync();
        if (!request.IsValid)
        {
            // Retornando os erros da validação.
            return request.ToFail<TokenResponse>();
        }

        var usuario = await _repository.ObterPorEmailAsync(new Email(request.Email));
        if (usuario == null)
            return Result.Fail<TokenResponse>(new NotFoundError("A conta informada não existe."));

        // Verificando se a conta está bloqueada.
        if (usuario.EstaBloqueado(_dateTime))
            return Result.Fail<TokenResponse>("A sua conta está bloqueada, entre em contato com o nosso suporte.");

        // Verificando se a senha corresponde a senha criptografada gravada na base de dados.
        if (_hashService.Compare(request.Password, usuario.HashSenha))
        {
            // Gerando as regras (roles).
            var claims = GenerateClaims(usuario);

            // Gerando o token de acesso.
            var (accessToken, createdAt, expiresAt) = _tokenClaimsService.GenerateAccessToken(claims);

            // Gerando o token de atualização.
            var refreshToken = _tokenClaimsService.GenerateRefreshToken();

            // Vinculando o token atualização ao usuário.
            usuario.AdicionarToken(new Token(accessToken, refreshToken, createdAt, expiresAt));

            _repository.Update(usuario);
            await _uow.SaveChangesAsync();

            return Result.Ok(new TokenResponse(accessToken, createdAt, expiresAt, refreshToken));
        }

        // Se o login for inválido, será incrementado o número de falhas,
        // se atingido o limite de tentativas de acesso a conta será bloqueada por um determinado tempo.
        var lockedTimeSpan = TimeSpan.FromSeconds(_authConfig.SecondsBlocked);
        usuario.IncrementarFalhas(_dateTime, _authConfig.MaximumAttempts, lockedTimeSpan);

        _repository.Update(usuario);
        await _uow.SaveChangesAsync();

        return Result.Fail<TokenResponse>("O e-mail ou senha está incorreta.");
    }

    public async Task<Result<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        // Validando a requisição.
        await request.ValidateAsync();
        if (!request.IsValid)
        {
            // Retornando os erros da validação.
            return request.ToFail<TokenResponse>();
        }

        var usuario = await _repository.ObterPorTokenAtualizacaoAsync(request.Token);
        if (usuario == null)
            return Result.Fail<TokenResponse>(new NotFoundError("Nenhum token encontrado."));

        // Verificando se o token de atualização está expirado.
        var token = usuario.Tokens.FirstOrDefault(t => t.Atualizacao == request.Token);
        if (token?.EstaValido(_dateTime) != true)
            return Result.Fail<TokenResponse>("O token inválido ou expirado.");

        // Gerando as regras (roles).
        var claims = GenerateClaims(usuario);

        // Gerando um novo token de acesso.
        var (accessToken, createdAt, expiresAt) = _tokenClaimsService.GenerateAccessToken(claims);

        // Revogando (cancelando) o token de atualização atual.
        token.Revogar(createdAt);

        // Gerando um novo token de atualização.
        var newRefreshToken = _tokenClaimsService.GenerateRefreshToken();

        // Vinculando o novo token atualização ao usuário.
        usuario.AdicionarToken(new Token(accessToken, newRefreshToken, createdAt, expiresAt));

        _repository.Update(usuario);
        await _uow.SaveChangesAsync();

        return Result.Ok(new TokenResponse(accessToken, createdAt, expiresAt, newRefreshToken));
    }

    private static Claim[] GenerateClaims(Usuario usuario) => new[]
    {
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
        new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Sub, usuario.Nome, ClaimValueTypes.String),
        new Claim(JwtRegisteredClaimNames.Email, usuario.Email.ToString(), ClaimValueTypes.Email)
    };

    #endregion
}