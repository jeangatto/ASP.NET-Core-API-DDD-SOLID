using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SGP.Shared.Abstractions;
using SGP.Shared.AppSettings;
using SGP.Shared.Extensions;

namespace SGP.Infrastructure.Services;

public class MemoryCacheService(
    ILogger<MemoryCacheService> logger,
    IMemoryCache memoryCache,
    IOptions<CacheOptions> cacheOptions) : ICacheService
{
    private readonly MemoryCacheEntryOptions _cacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(cacheOptions.Value.AbsoluteExpirationInHours),
        SlidingExpiration = TimeSpan.FromSeconds(cacheOptions.Value.SlidingExpirationInSeconds)
    };

    public async Task<TItem> GetOrCreateAsync<TItem>(string cacheKey, Func<Task<TItem>> factory)
    {
        return await memoryCache.GetOrCreateAsync(cacheKey, async cacheEntry =>
        {
            var cacheValue = cacheEntry?.Value;
            if (cacheValue != null)
            {
                logger.LogInformation("----- Fetched from MemoryCache: '{CacheKey}'", cacheKey);
                return (TItem)cacheValue;
            }

            var item = await factory();
            if (!item.IsDefault())
            {
                logger.LogInformation("----- Added to MemoryCache: '{CacheKey}'", cacheKey);
                memoryCache.Set(cacheKey, item, _cacheOptions);
            }

            return item;
        });
    }

    public Task RemoveAsync(string cacheKey)
    {
        logger.LogInformation("----- Removed from MemoryCache: '{Key}'", cacheKey);
        memoryCache.Remove(cacheKey);
        return Task.CompletedTask;
    }
}