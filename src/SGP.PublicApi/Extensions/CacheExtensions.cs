using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGP.Infrastructure;
using SGP.Shared.AppSettings;
using SGP.Shared.Extensions;

namespace SGP.PublicApi.Extensions;

[ExcludeFromCodeCoverage]
internal static class CacheExtensions
{
    private const string RedisInstanceName = "master";

    internal static IServiceCollection AddCacheService(
        this IServiceCollection services,
        IConfiguration configuration,
        IHealthChecksBuilder healthChecksBuilder)
    {
        var inMemoryOptions = configuration.GetOptions<InMemoryOptions>();
        if (inMemoryOptions.Cache)
        {
            services.AddMemoryCacheService();
            services.AddMemoryCache(memoryOptions => memoryOptions.TrackStatistics = true);
        }
        else
        {
            var connections = configuration.GetOptions<ConnectionStrings>();

            services.AddDistributedCacheService();
            services.AddDistributedRedisCache(options =>
            {
                options.InstanceName = RedisInstanceName;
                options.Configuration = connections.Cache;
            });

            healthChecksBuilder.AddRedis(connections.Cache);
        }

        return services;
    }
}