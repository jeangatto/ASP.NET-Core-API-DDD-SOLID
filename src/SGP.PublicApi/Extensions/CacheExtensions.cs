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
    internal static IServiceCollection AddCacheService(
        this IServiceCollection services,
        IConfiguration configuration,
        IHealthChecksBuilder healthChecksBuilder)
    {
        var inMemoryOptions = configuration.GetOptions<InMemoryOptions>();
        if (inMemoryOptions.Cache)
        {
            services
                .AddMemoryCache()
                .AddMemoryCacheService();
        }
        else
        {
            var connections = configuration.GetOptions<ConnectionStrings>();

            services.AddDistributedRedisCache(options =>
            {
                options.InstanceName = "master";
                options.Configuration = connections.Cache;
            }).AddDistributedCacheService();

            healthChecksBuilder.AddRedis(connections.Cache);
        }

        return services;
    }
}
