namespace SGP.Shared.AppSettings
{
    public sealed class AuthConfig
    {
        public short MaximumAttempts { get; private set; }
        public short SecondsBlocked { get; private set; }
    }
}
