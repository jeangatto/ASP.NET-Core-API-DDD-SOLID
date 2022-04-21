namespace SGP.Shared.AppSettings;

public sealed class JwtConfig
{
    /// <summary>
    /// aud: Define quem pode usar o token.
    /// </summary>
    public string Audience { get; private set; }

    /// <summary>
    /// iss: O domínio da aplicação geradora do token.
    /// </summary>
    public string Issuer { get; private set; }

    /// <summary>
    /// tempo de vida do token em segundos.
    /// </summary>
    public int Seconds { get; private set; }

    public string Secret { get; private set; }

    public static JwtConfig Create(string audience, string issuer, int seconds, string secret) => new()
    {
        Audience = audience, Issuer = issuer, Seconds = seconds, Secret = secret
    };
}