using SGP.Shared.Validation;

namespace SGP.Shared.AppSettings;

public sealed class CacheConfig
{
    [RequiredGreaterThanZero]
    public int AbsoluteExpirationInHours { get; private set; }

    [RequiredGreaterThanZero]
    public int SlidingExpirationInSeconds { get; private set; }
}