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

public class DistributedCacheService : ICacheService
{
    private readonly DistributedCacheEntryOptions _cacheOptions;
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<DistributedCacheService> _logger;

    public DistributedCacheService(
        ILogger<DistributedCacheService> logger,
        IDistributedCache distributedCache,
        IOptions<CacheOptions> cacheOptions)
    {
        _logger = logger;
        _distributedCache = distributedCache;
        _cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(cacheOptions.Value.AbsoluteExpirationInHours),
            SlidingExpiration = TimeSpan.FromSeconds(cacheOptions.Value.SlidingExpirationInSeconds)
        };
    }

    public async Task<TItem> GetOrCreateAsync<TItem>(string cacheKey, Func<Task<TItem>> factory)
    {
        var result = await _distributedCache.GetAsync(cacheKey);
        if (result?.Length > 0)
        {
            _logger.LogInformation("----- Fetched from DistributedCache: '{CacheKey}'", cacheKey);

            var value = Encoding.UTF8.GetString(result);
            return value.FromJson<TItem>();
        }

        var item = await factory();
        if (item != null)
        {
            _logger.LogInformation("----- Added to DistributedCache: '{CacheKey}'", cacheKey);

            var value = item.ToJson();
            var cacheValue = Encoding.UTF8.GetBytes(value);
            await _distributedCache.SetAsync(cacheKey, cacheValue, _cacheOptions);
        }

        return item;
    }

    public async Task RemoveAsync(string cacheKey)
    {
        _logger.LogInformation("----- Removed from DistributedCache: '{CacheKey}'", cacheKey);
        await _distributedCache.RemoveAsync(cacheKey);
    }
}