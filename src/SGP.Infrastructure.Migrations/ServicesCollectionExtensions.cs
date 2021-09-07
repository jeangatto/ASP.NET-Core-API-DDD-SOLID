using System;
using System.Reflection;
using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SGP.Infrastructure.Context;
using SGP.Shared.AppSettings;

namespace SGP.Infrastructure.Migrations
{
    public static class ServicesCollectionExtensions
    {
        private static readonly string AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;

        public static IServiceCollection AddDbContext(this IServiceCollection services,
            IHealthChecksBuilder healthChecksBuilder)
        {
            Guard.Against.Null(healthChecksBuilder, nameof(healthChecksBuilder));

            services.AddDbContext<SgpContext>((serviceProvider, builder) =>
            {
                builder.UseSqlServer(serviceProvider.GetConnectionString(),
                    options => options.MigrationsAssembly(AssemblyName));

                // NOTE: Quando for ambiente de desenvolvimento será logado informações detalhadas.
                var environment = serviceProvider.GetRequiredService<IHostEnvironment>();
                if (environment.IsDevelopment())
                {
                    builder
                        .UseLoggerFactory(LoggerFactory.Create(logging => logging.AddConsole()))
                        .EnableDetailedErrors()
                        .EnableSensitiveDataLogging();
                }
            });

            // Verificador de saúde da base de dados.
            healthChecksBuilder.AddDbContextCheck<SgpContext>(
                tags: new[] { "database" },
                customTestQuery: (context, cancellationToken)
                    => context.Estados.AsNoTracking().AnyAsync(cancellationToken));

            return services;
        }

        private static string GetConnectionString(this IServiceProvider serviceProvider)
            => serviceProvider.GetRequiredService<IOptions<ConnectionStrings>>().Value.DefaultConnection;
    }
}