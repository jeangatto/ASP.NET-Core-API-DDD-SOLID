using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGP.Infrastructure.Context;
using SGP.Shared.AppSettings;
using System.Reflection;

namespace SGP.Infrastructure.Migrations
{
    public static class DependencyInjection
    {
        private static readonly string AssymbleName = Assembly.GetExecutingAssembly().GetName().Name;

        public static IServiceCollection AddDbContext(this IServiceCollection services,
            IConfiguration configuration, IHealthChecksBuilder healthChecksBuilder)
        {
            Guard.Against.Null(services, nameof(services));
            Guard.Against.Null(configuration, nameof(configuration));
            Guard.Against.Null(healthChecksBuilder, nameof(healthChecksBuilder));

            var connectionString = configuration.GetConnectionString(
                nameof(ConnectionStrings.DefaultConnection));

            services.AddDbContext<SgpContext>(options
                => options.UseSqlServer(connectionString,
                       sqlServer => sqlServer.MigrationsAssembly(AssymbleName)));

            // Verificador da saúde da base de dados.
            healthChecksBuilder.AddDbContextCheck<SgpContext>(
                tags: new[] { "database" },
                customTestQuery: (context, cancellationToken)
                    => context.Cidades.AsNoTracking().AnyAsync(cancellationToken));

            return services;
        }
    }
}