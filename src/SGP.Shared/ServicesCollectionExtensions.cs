using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using SGP.Shared.Abstractions;
using SGP.Shared.AppSettings;
using SGP.Shared.Constants;

namespace SGP.Shared;

[ExcludeFromCodeCoverage]
public static class ServicesCollectionExtensions
{
    public static void ConfigureAppSettings(this IServiceCollection services) =>
        services
            .AddOptions<AuthOptions>(AppSettingsKeys.AuthOptions)
            .AddOptions<CacheOptions>(AppSettingsKeys.CacheOptions)
            .AddOptions<ConnectionStrings>(AppSettingsKeys.ConnectionStrings)
            .AddOptions<InMemoryOptions>(AppSettingsKeys.InMemoryOptions)
            .AddOptions<JwtOptions>(AppSettingsKeys.JwtOptions);

    private static IServiceCollection AddOptions<TOptions>(this IServiceCollection services, string configSectionPath)
        where TOptions : class, IAppOptions
    {
        return services
            .AddOptions<TOptions>()
            .BindConfiguration(configSectionPath, options => options.BindNonPublicProperties = true)
            .ValidateDataAnnotations()
            .ValidateOnStart()
            .Services;
    }
}