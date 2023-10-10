using SGP.Shared.Abstractions;
using SGP.Shared.ValidationAttributes;

namespace SGP.Shared.AppSettings;

public sealed class AuthOptions : IAppOptions
{
    public static string ConfigSectionPath => "AuthOptions";

    [RequiredGreaterThanZero]
    public int MaximumAttempts { get; private init; }

    [RequiredGreaterThanZero]
    public int SecondsBlocked { get; private init; }

    public static AuthOptions Create(int maximumAttempts, int secondsBlocked)
        => new() { MaximumAttempts = maximumAttempts, SecondsBlocked = secondsBlocked };
}