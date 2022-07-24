using Microsoft.Extensions.DependencyInjection;
using SGP.Shared.AppSettings;

namespace SGP.Shared;

public static class ServicesCollectionExtensions
{
    public static IServiceCollection ConfigureAppSettings(this IServiceCollection services)
    {
        services.AddOptions<AuthConfig>()
            .BindConfiguration(nameof(AuthConfig), options => options.BindNonPublicProperties = true)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<CacheConfig>()
            .BindConfiguration(nameof(CacheConfig), options => options.BindNonPublicProperties = true)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<ConnectionStrings>()
            .BindConfiguration(nameof(ConnectionStrings), options => options.BindNonPublicProperties = true)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<JwtConfig>()
            .BindConfiguration(nameof(JwtConfig), options => options.BindNonPublicProperties = true)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}