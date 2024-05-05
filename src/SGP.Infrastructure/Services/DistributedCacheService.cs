using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SGP.Shared.Abstractions;
using SGP.Shared.AppSettings;
using SGP.Shared.Extensions;

namespace SGP.Infrastructure.Services;

public class DistributedCacheService(
    ILogger<DistributedCacheService> logger,
    IDistributedCache distributedCache,
    IOptions<CacheOptions> cacheOptions) : ICacheService
{
    private readonly DistributedCacheEntryOptions _cacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(cacheOptions.Value.AbsoluteExpirationInHours),
        SlidingExpiration = TimeSpan.FromSeconds(cacheOptions.Value.SlidingExpirationInSeconds)
    };

    public async Task<TItem> GetOrCreateAsync<TItem>(string cacheKey, Func<Task<TItem>> factory)
    {
        var result = await distributedCache.GetAsync(cacheKey);
        if (result?.Length > 0)
        {
            logger.LogInformation("----- Fetched from DistributedCache: '{CacheKey}'", cacheKey);

            var value = Encoding.UTF8.GetString(result);
            return value.FromJson<TItem>();
        }

        var item = await factory();
        if (!item.IsDefault())
        {
            logger.LogInformation("----- Added to DistributedCache: '{CacheKey}'", cacheKey);

            var value = item.ToJson();
            var cacheValue = Encoding.UTF8.GetBytes(value);
            await distributedCache.SetAsync(cacheKey, cacheValue, _cacheOptions);
        }

        return item;
    }

    public async Task RemoveAsync(string cacheKey)
    {
        logger.LogInformation("----- Removed from DistributedCache: '{CacheKey}'", cacheKey);
        await distributedCache.RemoveAsync(cacheKey);
    }
}