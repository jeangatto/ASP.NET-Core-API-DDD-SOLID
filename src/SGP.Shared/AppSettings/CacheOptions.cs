using SGP.Shared.Validation;

namespace SGP.Shared.AppSettings;

public sealed class CacheOptions
{
    public const string ConfigSectionPath = "CacheConfig";

    [RequiredGreaterThanZero]
    public int AbsoluteExpirationInHours { get; private init; }

    [RequiredGreaterThanZero]
    public int SlidingExpirationInSeconds { get; private init; }
}