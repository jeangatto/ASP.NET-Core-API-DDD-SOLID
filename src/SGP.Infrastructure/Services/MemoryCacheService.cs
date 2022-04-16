using System;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SGP.Shared.AppSettings;
using SGP.Shared.Interfaces;

namespace SGP.Infrastructure.Services;

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly MemoryCacheEntryOptions _cacheOptions;

    public MemoryCacheService(IMemoryCache memoryCache, IOptions<CacheConfig> options)
    {
        _memoryCache = memoryCache;
        _cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(options.Value.AbsoluteExpirationInHours),
            SlidingExpiration = TimeSpan.FromSeconds(options.Value.SlidingExpirationInSeconds)
        };
    }

    public async Task<TItem> GetOrCreateAsync<TItem>(string cacheKey, Func<Task<TItem>> factory)
    {
        if (!_memoryCache.TryGetValue(cacheKey, out object value))
        {
            value = await factory().ConfigureAwait(false);
            _memoryCache.Set(cacheKey, value, _cacheOptions);
        }

        return (TItem)value;
    }

    public void Remove(string cacheKey)
    {
        Guard.Against.NullOrWhiteSpace(cacheKey, nameof(cacheKey));
        _memoryCache.Remove(cacheKey);
    }
}