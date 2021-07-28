using System;
using Microsoft.Extensions.Caching.Memory;

namespace SGP.Application.Services.Common
{
    public abstract class BaseService
    {
        protected readonly IMemoryCache MemoryCache;

        protected BaseService(IMemoryCache memoryCache)
        {
            MemoryCache = memoryCache;
        }

        protected static void ConfigureCacheEntry(ICacheEntry cacheEntry)
        {
            cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(60);
            cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2);
        }
    }
}