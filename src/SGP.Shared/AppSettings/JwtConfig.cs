namespace SGP.Shared.AppSettings
{
    public sealed class JwtConfig
    {
        public string Audience { get; private set; }
        public string Issuer { get; private set; }
        public short Seconds { get; private set; }
        public string Secret { get; private set; }
    }
}
