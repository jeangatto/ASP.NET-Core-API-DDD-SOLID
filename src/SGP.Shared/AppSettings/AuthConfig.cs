using SGP.Shared.Validation;

namespace SGP.Shared.AppSettings;

public sealed class AuthConfig
{
    [RequiredGreaterThanZero]
    public int MaximumAttempts { get; private set; }

    [RequiredGreaterThanZero]
    public int SecondsBlocked { get; private set; }

    public static AuthConfig Create(int maximumAttempts, int secondsBlocked)
        => new() { MaximumAttempts = maximumAttempts, SecondsBlocked = secondsBlocked };
}