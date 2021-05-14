using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SGP.Infrastructure.Context;
using SGP.Shared.AppSettings;
using System;

namespace SGP.Infrastructure.Migrations
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services,
            IConfiguration configuration,
            IHealthChecksBuilder healthChecksBuilder)
        {
            Guard.Against.Null(services, nameof(services));
            Guard.Against.Null(configuration, nameof(configuration));
            Guard.Against.Null(healthChecksBuilder, nameof(healthChecksBuilder));

            // Obtendo o tipo de ambiente.
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Obtendo a string de conexão.
            var connectionString = configuration.GetConnectionString(
                nameof(ConnectionStrings.DefaultConnection));

            services.AddDbContext<SgpContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(connectionString, sqlServerBuilder
                    => sqlServerBuilder.MigrationsAssembly(MigrationsOptions.AssemblyName));

                // Configurando para exibir os errados mais detalhados.
                // NOTE: recomendado o uso somente para ambiente de desenvolvimento.
                if (environment == Environments.Development)
                {
                    optionsBuilder.UseLoggerFactory(MigrationsOptions.LoggerDbFactory);
                    optionsBuilder.EnableDetailedErrors();
                    optionsBuilder.EnableSensitiveDataLogging();
                }
            });

            // Verificador de saúde da base de dados.
            healthChecksBuilder.AddDbContextCheck<SgpContext>(
                tags: new[] { "database" },
                customTestQuery: (context, cancellationToken)
                    => context.Cities.AsNoTracking().AnyAsync(cancellationToken));

            return services;
        }
    }
}