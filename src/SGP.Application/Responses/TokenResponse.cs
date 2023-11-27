using System;
using SGP.Shared.Messages;

namespace SGP.Application.Responses;

public sealed class TokenResponse(string accessToken, DateTime created, DateTime expiration, string refreshToken) : IResponse
{
    /// <summary>
    /// Token de acesso.
    /// </summary>
    public string AccessToken { get; } = accessToken;

    /// <summary>
    /// Data da criação do token.
    /// </summary>
    public DateTime Created { get; } = created;

    /// <summary>
    /// Data do vencimento do token.
    /// </summary>
    public DateTime Expiration { get; } = expiration;

    /// <summary>
    /// Token de atualização.
    /// </summary>
    public string RefreshToken { get; } = refreshToken;

    /// <summary>
    /// Expiração do token em segundos.
    /// </summary>
    public int ExpiresIn => (int)Expiration.Subtract(Created).TotalSeconds;
}