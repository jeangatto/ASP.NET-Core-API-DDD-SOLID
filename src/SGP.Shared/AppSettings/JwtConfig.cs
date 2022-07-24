using System.ComponentModel.DataAnnotations;
using SGP.Shared.Validation;

namespace SGP.Shared.AppSettings;

public sealed class JwtConfig
{
    /// <summary>
    /// aud: Define quem pode usar o token.
    /// </summary>
    [Required]
    public string Audience { get; private set; }

    /// <summary>
    /// iss: O domínio da aplicação geradora do token.
    /// </summary>
    [Required]
    public string Issuer { get; private set; }

    /// <summary>
    /// tempo de vida do token em segundos.
    /// </summary>
    [RequiredGreaterThanZero]
    public int Seconds { get; private set; }

    [Required]
    public string Secret { get; private set; }

    public static JwtConfig Create(string audience, string issuer, int seconds, string secret) => new()
    {
        Audience = audience,
        Issuer = issuer,
        Seconds = seconds,
        Secret = secret
    };
}