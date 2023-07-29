using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using SGP.Shared.Abstractions;
using SGP.Shared.AppSettings;

namespace SGP.Shared;

[ExcludeFromCodeCoverage]
public static class ServicesCollectionExtensions
{
    public static IServiceCollection ConfigureAppSettings(this IServiceCollection services) =>
        services
            .AddOptionsWithValidation<AuthOptions>()
            .AddOptionsWithValidation<CacheOptions>()
            .AddOptionsWithValidation<ConnectionStrings>()
            .AddOptionsWithValidation<InMemoryOptions>()
            .AddOptionsWithValidation<JwtOptions>();

    private static IServiceCollection AddOptionsWithValidation<TOptions>(this IServiceCollection services)
        where TOptions : class, IAppOptions
    {
        return services
            .AddOptions<TOptions>()
            .BindConfiguration(TOptions.ConfigSectionPath, binderOptions => binderOptions.BindNonPublicProperties = true)
            .ValidateDataAnnotations()
            .ValidateOnStart()
            .Services;
    }
}