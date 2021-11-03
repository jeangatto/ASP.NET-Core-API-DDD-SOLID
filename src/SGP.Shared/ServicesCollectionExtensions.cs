using IL.FluentValidation.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGP.Shared.AppSettings;
using SGP.Shared.AppSettings.OptionsValidators;

namespace SGP.Shared
{
    public static class ServicesCollectionExtensions
    {
        private static void BinderOptionsNonPublicProperties(BinderOptions options) =>
            options.BindNonPublicProperties = true;

        public static IServiceCollection ConfigureAppSettings(this IServiceCollection services)
        {
            services.AddOptions<AuthConfig>()
                .BindConfiguration(nameof(AuthConfig), BinderOptionsNonPublicProperties)
                .FluentValidate().With<AuthConfigValidator>();

            services.AddOptions<JwtConfig>()
                .BindConfiguration(nameof(JwtConfig), BinderOptionsNonPublicProperties)
                .FluentValidate().With<JwtConfigValidator>();

            services.AddOptions<ConnectionStrings>()
                .BindConfiguration(nameof(ConnectionStrings), BinderOptionsNonPublicProperties)
                .FluentValidate().With<ConnectionStringsValidator>();

            return services;
        }
    }
}