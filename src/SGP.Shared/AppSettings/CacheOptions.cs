using SGP.Shared.Abstractions;
using SGP.Shared.ValidationAttributes;

namespace SGP.Shared.AppSettings;

public sealed class CacheOptions : IAppOptions
{
    public static string ConfigSectionPath => "CacheOptions";

    [RequiredGreaterThanZero]
    public int AbsoluteExpirationInHours { get; private init; }

    [RequiredGreaterThanZero]
    public int SlidingExpirationInSeconds { get; private init; }
}
