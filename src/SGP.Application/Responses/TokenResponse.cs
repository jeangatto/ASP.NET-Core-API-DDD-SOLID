using System;
using SGP.Shared.Messages;

namespace SGP.Application.Responses;

public sealed class TokenResponse : IResponse
{
    public TokenResponse(string accessToken, DateTime created, DateTime expiration, string refreshToken)
    {
        AccessToken = accessToken;
        Created = created;
        Expiration = expiration;
        RefreshToken = refreshToken;
    }

    /// <summary>
    /// Token de acesso.
    /// </summary>
    public string AccessToken { get; }

    /// <summary>
    /// Data da criação do token.
    /// </summary>
    public DateTime Created { get; }

    /// <summary>
    /// Data do vencimento do token.
    /// </summary>
    public DateTime Expiration { get; }

    /// <summary>
    /// Token de atualização.
    /// </summary>
    public string RefreshToken { get; }

    /// <summary>
    /// Expiração do token em segundos.
    /// </summary>
    public int ExpiresIn => (int)Expiration.Subtract(Created).TotalSeconds;
}
