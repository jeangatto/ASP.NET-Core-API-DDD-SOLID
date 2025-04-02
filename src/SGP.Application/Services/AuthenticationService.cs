#region

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using Microsoft.Extensions.Options;
using SGP.Application.Interfaces;
using SGP.Application.Requests.AuthenticationRequests;
using SGP.Application.Responses;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Domain.ValueObjects;
using SGP.Shared.Abstractions;
using SGP.Shared.AppSettings;

#endregion

namespace SGP.Application.Services;

public class AuthenticationService(
    IOptions<AuthOptions> authOptions,
    IDateTimeService dateTimeService,
    IHashService hashService,
    ITokenClaimsService tokenClaimsService,
    IUsuarioRepository usuarioRepository,
    IUnitOfWork uow) : IAuthenticationService
{
    #region Fields

    private readonly AuthOptions _authOptions = authOptions.Value;

    #endregion

    #region Methods

    public async Task<Result<TokenResponse>> AuthenticateAsync(LogInRequest request)
    {
        await request.ValidateAsync();
        if (!request.IsValid)
            return Result.Invalid(request.ValidationResult.AsErrors());

        var usuario = await usuarioRepository.ObterPorEmailAsync(new Email(request.Email));
        if (usuario == null)
            return Result.NotFound("A conta informada não existe.");

        // Verificando se a conta está bloqueada.
        if (usuario.EstaBloqueado(dateTimeService))
            return Result.Error("A sua conta está bloqueada, entre em contato com o nosso suporte.");

        // Verificando se a senha corresponde a senha criptografada gravada na base de dados.
        if (hashService.Compare(request.Password, usuario.HashSenha))
        {
            // Gerando as regras (roles).
            var claims = GenerateClaims(usuario);

            // Gerando o token de acesso.
            var (accessToken, createdAt, expiresAt) = tokenClaimsService.GenerateAccessToken(claims);

            // Gerando o token de atualização.
            var refreshToken = tokenClaimsService.GenerateRefreshToken();

            // Vinculando o token atualização ao usuário.
            usuario.AdicionarToken(new Token(accessToken, refreshToken, createdAt, expiresAt));

            usuarioRepository.Update(usuario);
            await uow.CommitAsync();

            return Result.Success(new TokenResponse(accessToken, createdAt, expiresAt, refreshToken));
        }

        // Se o login for inválido, será incrementado o número de falhas,
        // se atingido o limite de tentativas de acesso a conta será bloqueada por um determinado tempo.
        var lockedTimeSpan = TimeSpan.FromSeconds(_authOptions.SecondsBlocked);
        usuario.IncrementarFalhas(dateTimeService, _authOptions.MaximumAttempts, lockedTimeSpan);

        usuarioRepository.Update(usuario);
        await uow.CommitAsync();

        return Result.Error("O e-mail ou senha está incorreta.");
    }

    public async Task<Result<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        await request.ValidateAsync();
        if (!request.IsValid)
            return Result.Invalid(request.ValidationResult.AsErrors());

        var usuario = await usuarioRepository.ObterPorTokenAtualizacaoAsync(request.Token);
        if (usuario == null)
            return Result.NotFound("Nenhum token encontrado.");

        // Verificando se o token de atualização está expirado.
        var token = usuario.Tokens.FirstOrDefault(t => t.Atualizacao == request.Token);
        if (token?.EstaValido(dateTimeService) != false)
            return Result.Error("O token inválido ou expirado.");

        // Gerando as regras (roles).
        var claims = GenerateClaims(usuario);

        // Gerando um novo token de acesso.
        var (accessToken, createdAt, expiresAt) = tokenClaimsService.GenerateAccessToken(claims);

        // Revogando (cancelando) o token de atualização atual.
        token!.Revogar(createdAt);

        // Gerando um novo token de atualização.
        var newRefreshToken = tokenClaimsService.GenerateRefreshToken();

        // Vinculando o novo token atualização ao usuário.
        usuario.AdicionarToken(new Token(accessToken, newRefreshToken, createdAt, expiresAt));

        usuarioRepository.Update(usuario);
        await uow.CommitAsync();

        return Result.Success(new TokenResponse(accessToken, createdAt, expiresAt, newRefreshToken));
    }

    private static Claim[] GenerateClaims(Usuario usuario) =>
    [
        new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
        new(JwtRegisteredClaimNames.UniqueName, usuario.Id.ToString()),
        new(JwtRegisteredClaimNames.Sub, usuario.Nome, ClaimValueTypes.String),
        new(JwtRegisteredClaimNames.Email, usuario.Email.ToString(), ClaimValueTypes.Email)
    ];

    #endregion
}