namespace SGP.Shared.AppSettings;

public sealed class AuthConfig
{
    public int MaximumAttempts { get; private set; }
    public int SecondsBlocked { get; private set; }

    public static AuthConfig Create(int maximumAttempts, int secondsBlocked)
        => new() { MaximumAttempts = maximumAttempts, SecondsBlocked = secondsBlocked };
}