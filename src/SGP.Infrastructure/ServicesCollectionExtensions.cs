using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using SGP.Shared.AppSettings;
using SGP.Shared.Interfaces;
using System;

namespace SGP.Infrastructure
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            Guard.Against.Null(services, nameof(services));

            // Automatically register services ASP.NET Core DI container
            // REF: https://andrewlock.net/using-scrutor-to-automatically-register-your-services-with-the-asp-net-core-di-container/
            // REF: https://github.com/khellang/Scrutor
            services.Scan(scan => scan.FromCallingAssembly()

                    // Services
                    .AddClasses(classes => classes.Where(t => t.Name.EndsWith("Service")))
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsImplementedInterfaces()
                        .WithScopedLifetime()

                    // Repositories
                    .AddClasses(classes => classes.AssignableTo<IRepository>())
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsImplementedInterfaces()
                        .WithScopedLifetime()

                    // Unit Of Work
                    .AddClasses(classes => classes.AssignableTo<IUnitOfWork>())
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsImplementedInterfaces()
                        .WithScopedLifetime()
            );

            return services;
        }

        public static IServiceCollection ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            Guard.Against.Null(services, nameof(services));
            Guard.Against.Null(configuration, nameof(configuration));

            var binderOptions = ConfigureBinderOptions();
            services.Configure<JwtConfig>(configuration.GetSection(nameof(JwtConfig)), binderOptions);
            services.Configure<AuthConfig>(configuration.GetSection(nameof(AuthConfig)), binderOptions);
            services.Configure<ConnectionStrings>(configuration.GetSection(nameof(ConnectionStrings)), binderOptions);
            return services;
        }

        private static Action<BinderOptions> ConfigureBinderOptions()
        {
            return binderOptions => binderOptions.BindNonPublicProperties = true;
        }
    }
}