using Microsoft.Extensions.DependencyInjection;
using SGP.Shared.AppSettings;

namespace SGP.Shared;

public static class ServicesCollectionExtensions
{
    public static IServiceCollection ConfigureAppSettings(this IServiceCollection services)
    {
        services.AddOptions<AuthOptions>(AuthOptions.ConfigSectionPath);
        services.AddOptions<CacheOptions>(CacheOptions.ConfigSectionPath);
        services.AddOptions<ConnectionOptions>(ConnectionOptions.ConfigSectionPath);
        services.AddOptions<JwtOptions>(JwtOptions.ConfigSectionPath);
        services.AddOptions<RootOptions>(RootOptions.ConfigSectionPath);
        return services;
    }

    private static void AddOptions<T>(this IServiceCollection services, string configSectionPath) where T : class
        => services
            .AddOptions<T>()
            .BindConfiguration(configSectionPath, options => options.BindNonPublicProperties = true)
            .ValidateDataAnnotations()
            .ValidateOnStart();
}