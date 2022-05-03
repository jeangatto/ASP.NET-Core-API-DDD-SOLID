namespace SGP.Shared.AppSettings;

public class CacheConfig
{
    public int AbsoluteExpirationInHours { get; private set; }
    public int SlidingExpirationInSeconds { get; private set; }
}