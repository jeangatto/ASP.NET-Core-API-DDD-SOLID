using IL.FluentValidation.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGP.Shared.AppSettings;
using SGP.Shared.AppSettings.OptionsValidators;

namespace SGP.Shared;

public static class ServicesCollectionExtensions
{
    public static IServiceCollection ConfigureAppSettings(this IServiceCollection services)
    {
        services.AddOptions<AuthConfig>()
            .BindConfiguration(nameof(AuthConfig), BinderOptions)
            .FluentValidate()
            .With<AuthConfigValidator>();

        services.AddOptions<JwtConfig>()
            .BindConfiguration(nameof(JwtConfig), BinderOptions)
            .FluentValidate()
            .With<JwtConfigValidator>();

        services.AddOptions<ConnectionStrings>()
            .BindConfiguration(nameof(ConnectionStrings), BinderOptions)
            .FluentValidate()
            .With<ConnectionStringsValidator>();

        return services;
    }

    private static void BinderOptions(BinderOptions options) => options.BindNonPublicProperties = true;
}