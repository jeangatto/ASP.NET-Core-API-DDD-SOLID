using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using SGP.Shared.Interfaces;

namespace SGP.Infrastructure.Services
{
    public class MemoryCacheService : ICacheService
    {
        private static readonly MemoryCacheEntryOptions CacheOptions = new()
        {
            SlidingExpiration = TimeSpan.FromSeconds(60),
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2),
            Priority = CacheItemPriority.High
        };

        private readonly IMemoryCache _memoryCache;

        public MemoryCacheService(IMemoryCache memoryCache) => _memoryCache = memoryCache;

        public async Task<TItem> GetOrCreateAsync<TItem>(string cacheKey, Func<Task<TItem>> factory)
        {
            if (!_memoryCache.TryGetValue(cacheKey, out object result))
            {
                result = await factory().ConfigureAwait(false);
                _memoryCache.Set(cacheKey, result, CacheOptions);
            }

            return (TItem)result;
        }
    }
}