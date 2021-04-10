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

            // Unit Of Work
            services.AddScoped<IUnitOfWork, UnitOfWork<SgpContext>>();

            // Repositories
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<ICidadeRepository, CidadeRepository>();
            return services;
        }

        public static IServiceCollection ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            Guard.Against.Null(services, nameof(services));
            Guard.Against.Null(configuration, nameof(configuration));

            var configureBinder = ConfigureBinderOptions();
            services.Configure<AuthConfig>(configuration.GetSection(nameof(AuthConfig)), configureBinder);
            services.Configure<JwtConfig>(configuration.GetSection(nameof(JwtConfig)), configureBinder);
            services.Configure<ConnectionStrings>(configuration.GetSection(nameof(ConnectionStrings)), configureBinder);
            return services;
        }

        private static Action<BinderOptions> ConfigureBinderOptions()
        {
            return options => options.BindNonPublicProperties = true;
        }
    }
}