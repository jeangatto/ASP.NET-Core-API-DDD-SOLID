using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGP.Domain.Repositories;
using SGP.Infrastructure.Context;
using SGP.Infrastructure.Repositories;
using SGP.Infrastructure.Services;
using SGP.Shared.AppSettings;
using SGP.Shared.Interfaces;
using SGP.Shared.UnitOfWork;
using System;

namespace SGP.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            Guard.Against.Null(services, nameof(services));

            // Services
            services.AddScoped<IDateTime, LocalDateTimeService>();
            services.AddScoped<IHashService, BCryptHashService>();
            services.AddScoped<ITokenClaimsService, IdentityTokenClaimService>();

            // Unit Of Work
            services.AddScoped<IUnitOfWork, UnitOfWork<SgpContext>>();

            // Repositories
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }

        public static IServiceCollection ConfigureAppSettings(this IServiceCollection services,
            IConfiguration configuration)
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