using SGP.Shared.ValidationAttributes;

namespace SGP.Shared.AppSettings;

public sealed class AuthOptions
{
    public const string ConfigSectionPath = "AuthConfig";

    [RequiredGreaterThanZero]
    public int MaximumAttempts { get; private init; }

    [RequiredGreaterThanZero]
    public int SecondsBlocked { get; private init; }

    public static AuthOptions Create(int maximumAttempts, int secondsBlocked)
        => new() { MaximumAttempts = maximumAttempts, SecondsBlocked = secondsBlocked };
}