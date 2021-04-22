namespace SGP.Shared.AppSettings
{
    public sealed class AuthConfig
    {
        public short MaximumAttempts { get; private init; }
        public short SecondsBlocked { get; private init; }
    }
}