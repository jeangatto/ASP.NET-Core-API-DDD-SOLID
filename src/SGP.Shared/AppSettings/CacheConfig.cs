using SGP.Shared.Validation;

namespace SGP.Shared.AppSettings;

public sealed class CacheConfig
{
    [RequiredGreaterThanZero]
    public int AbsoluteExpirationInHours { get; private init; }

    [RequiredGreaterThanZero]
    public int SlidingExpirationInSeconds { get; private init; }
}