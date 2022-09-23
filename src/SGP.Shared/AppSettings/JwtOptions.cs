using System.ComponentModel.DataAnnotations;
using SGP.Shared.Validation;

namespace SGP.Shared.AppSettings;

public sealed class JwtOptions
{
    public const string ConfigSectionPath = "JwtConfig";

    /// <summary>
    /// aud: Define quem pode usar o token.
    /// </summary>
    [Required]
    public string Audience { get; private init; }

    /// <summary>
    /// iss: O domínio da aplicação geradora do token.
    /// </summary>
    [Required]
    public string Issuer { get; private init; }

    /// <summary>
    /// tempo de vida do token em segundos.
    /// </summary>
    [RequiredGreaterThanZero]
    public int Seconds { get; private init; }

    [Required]
    public string Secret { get; private init; }

    public static JwtOptions Create(string audience, string issuer, int seconds, string secret) => new()
    {
        Audience = audience,
        Issuer = issuer,
        Seconds = seconds,
        Secret = secret
    };
}