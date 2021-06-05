namespace SGP.Shared.AppSettings
{
    public sealed class JwtConfig
    {
        public string Audience { get; private set; }
        public string Issuer { get; private set; }
        public short Seconds { get; private set; }
        public string Secret { get; private set; }
        public bool ValidateAudience { get; private set; }
        public bool ValidateIssuer { get; private set; }

        public static JwtConfig Create(
            string audience,
            string issuer,
            short seconds,
            string secret,
            bool validateAudience,
            bool validateIssuer)
        {
            return new JwtConfig
            {
                Audience = audience,
                Issuer = issuer,
                Seconds = seconds,
                Secret = secret,
                ValidateAudience = validateAudience,
                ValidateIssuer = validateIssuer
            };
        }
    }
}