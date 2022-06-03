using IL.FluentValidation.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using SGP.Shared.AppSettings;
using SGP.Shared.AppSettings.Validators;

namespace SGP.Shared;

public static class ServicesCollectionExtensions
{
    public static IServiceCollection ConfigureAppSettings(this IServiceCollection services)
    {
        services.AddOptions<AuthConfig>()
            .BindConfiguration(nameof(AuthConfig), options => options.BindNonPublicProperties = true)
            .FluentValidate()
            .With<AuthConfigValidator>();

        services.AddOptions<CacheConfig>()
            .BindConfiguration(nameof(CacheConfig), options => options.BindNonPublicProperties = true)
            .FluentValidate()
            .With<CacheConfigValidator>();

        services.AddOptions<ConnectionStrings>()
            .BindConfiguration(nameof(ConnectionStrings), options => options.BindNonPublicProperties = true)
            .FluentValidate()
            .With<ConnectionStringsValidator>();

        services.AddOptions<JwtConfig>()
            .BindConfiguration(nameof(JwtConfig), options => options.BindNonPublicProperties = true)
            .FluentValidate()
            .With<JwtConfigValidator>();

        return services;
    }
}