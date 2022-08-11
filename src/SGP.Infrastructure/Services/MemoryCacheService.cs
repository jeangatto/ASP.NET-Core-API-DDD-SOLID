using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SGP.Shared.AppSettings;
using SGP.Shared.Interfaces;

namespace SGP.Infrastructure.Services;

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<MemoryCacheService> _logger;
    private readonly MemoryCacheEntryOptions _memoryCacheOptions;

    public MemoryCacheService(
        ILogger<MemoryCacheService> logger,
        IMemoryCache memoryCache,
        IOptions<CacheOptions> cacheOptions)
    {
        _logger = logger;
        _memoryCache = memoryCache;
        _memoryCacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(cacheOptions.Value.AbsoluteExpirationInHours),
            SlidingExpiration = TimeSpan.FromSeconds(cacheOptions.Value.SlidingExpirationInSeconds)
        };
    }

    public async Task<TItem> GetOrCreateAsync<TItem>(string cacheKey, Func<Task<TItem>> factory)
    {
        if (!_memoryCache.TryGetValue(cacheKey, out var value))
        {
            _logger.LogInformation("----- Added to cache: '{CacheKey}'", cacheKey);
            value = await factory().ConfigureAwait(false);
            _memoryCache.Set(cacheKey, value, _memoryCacheOptions);
            return (TItem)value;
        }

        _logger.LogInformation("----- Fetched from cache: '{CacheKey}'", cacheKey);
        return (TItem)value;
    }

    public void Remove(string cacheKey)
    {
        _logger.LogInformation("----- Removed from cache: '{CacheKey}'", cacheKey);
        _memoryCache.Remove(cacheKey);
    }
}