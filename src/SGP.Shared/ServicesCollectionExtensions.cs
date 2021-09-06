using System;
using IL.FluentValidation.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGP.Shared.AppSettings;
using SGP.Shared.AppSettings.OptionsValidators;

namespace SGP.Shared
{
    public static class ServicesCollectionExtensions
    {
        public static void ConfigureAppSettings(this IServiceCollection services)
        {
            var binderOptions = BinderOptions();

            services.AddOptions<AuthConfig>()
                .BindConfiguration(nameof(AuthConfig), binderOptions)
                .FluentValidate().With<AuthOptionsValidator>();

            services.AddOptions<JwtConfig>()
                .BindConfiguration(nameof(JwtConfig), binderOptions)
                .FluentValidate().With<JwtOptionsValidator>();

            services.AddOptions<ConnectionStrings>()
                .BindConfiguration(nameof(ConnectionStrings), binderOptions)
                .FluentValidate().With<ConnectionStringsOptionsValidator>();
        }

        private static Action<BinderOptions> BinderOptions()
            => options => options.BindNonPublicProperties = true;
    }
}