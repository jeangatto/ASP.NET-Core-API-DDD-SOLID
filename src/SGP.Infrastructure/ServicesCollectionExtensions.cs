using Ardalis.GuardClauses;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using SGP.Infrastructure.Services;
using SGP.Infrastructure.UoW;
using SGP.Shared.AppSettings;
using SGP.Shared.Interfaces;

namespace SGP.Infrastructure
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            Guard.Against.Null(services, nameof(services));

            services.AddScoped<IDateTime, LocalDateTimeService>();
            services.AddScoped<IHashService, BCryptHashService>();
            services.AddScoped<ITokenClaimsService, IdentityTokenClaimService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Automatically register services ASP.NET Core DI container
            // REF: https://github.com/khellang/Scrutor
            // REF: https://andrewlock.net/using-scrutor-to-automatically-register-your-services-with-the-asp-net-core-di-container/
            services.Scan(scan => scan
                .FromCallingAssembly()
                .AddClasses(implementations => implementations.AssignableTo<IRepository>())
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return services;
        }

        public static IServiceCollection ConfigureAppSettings(this IServiceCollection services)
        {
            Guard.Against.Null(services, nameof(services));

            services.AddOptions<AuthConfig>().BindConfiguration(nameof(AuthConfig),
                options => options.BindNonPublicProperties = true);

            services.AddOptions<JwtConfig>().BindConfiguration(nameof(JwtConfig),
                options => options.BindNonPublicProperties = true);

            services.AddOptions<ConnectionStrings>().BindConfiguration(nameof(ConnectionStrings),
                options => options.BindNonPublicProperties = true);

            return services;
        }
    }
}