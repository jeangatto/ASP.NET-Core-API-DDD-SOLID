namespace SGP.Shared.AppSettings
{
    public sealed class JwtConfig
    {
        public string Audience { get; private init; }
        public string Issuer { get; private init; }
        public short Seconds { get; private init; }
        public string Secret { get; private init; }
        public bool ValidateAudience { get; private init; }
        public bool ValidateIssuer { get; private init; }
    }
}