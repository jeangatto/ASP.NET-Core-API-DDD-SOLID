using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SGP.Shared.AppSettings;
using SGP.Shared.Extensions;
using SGP.Shared.Interfaces;

namespace SGP.Infrastructure.Services;

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<MemoryCacheService> _logger;
    private readonly MemoryCacheEntryOptions _cacheOptions;

    public MemoryCacheService(
        ILogger<MemoryCacheService> logger,
        IMemoryCache memoryCache,
        IOptions<CacheConfig> options)
    {
        _logger = logger;
        _memoryCache = memoryCache;
        _cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(options.Value.AbsoluteExpirationInHours),
            SlidingExpiration = TimeSpan.FromSeconds(options.Value.SlidingExpirationInSeconds)
        };
    }

    public async Task<TItem> GetOrCreateAsync<TItem>(string cacheKey, Func<Task<TItem>> factory)
    {
        if (!_memoryCache.TryGetValue(cacheKey, out object cachedValue))
        {
            _logger.LogInformation("Added to cache: '{CacheKey}'", cacheKey);
            cachedValue = await factory().ConfigureAwait(false);
            var serializedData = Encoding.UTF8.GetBytes(cachedValue.ToJson());
            _memoryCache.Set(cacheKey, serializedData, _cacheOptions);
            return (TItem)cachedValue;
        }
        else
        {
            _logger.LogInformation("Fetched from cache: '{CacheKey}'", cacheKey);
            var deserializedData = Encoding.UTF8.GetString(cachedValue as byte[]);
            return deserializedData.FromJson<TItem>();
        }
    }

    public void Remove(string cacheKey)
    {
        _logger.LogInformation("Removed from cache: '{CacheKey}'", cacheKey);
        _memoryCache.Remove(cacheKey);
    }
}