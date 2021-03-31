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
            // Services
            services.AddScoped<IDateTime, LocalDateTimeService>();
            services.AddScoped<IHashService, BCryptHashService>();
            services.AddScoped<IJWTokenService, JWTokenService>();

            // Unit Of Work
            services.AddScoped<IUnitOfWork, UnitOfWork<SgpContext>>();

            // Repositories
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<ICidadeRepository, CidadeRepository>();
            return services;
        }

        public static IServiceCollection ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            Guard.Against.Null(configuration, nameof(configuration));

            services.Configure<AuthConfig>(configuration.GetSection(nameof(AuthConfig)), BinderOptions());
            services.Configure<JwtConfig>(configuration.GetSection(nameof(JwtConfig)), BinderOptions());
            services.Configure<ConnectionStrings>(configuration.GetSection(nameof(ConnectionStrings)), BinderOptions());
            return services;
        }

        private static Action<BinderOptions> BinderOptions()
        {
            return options => options.BindNonPublicProperties = true;
        }
    }
}