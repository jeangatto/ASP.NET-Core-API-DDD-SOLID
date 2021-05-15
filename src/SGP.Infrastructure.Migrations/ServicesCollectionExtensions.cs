using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SGP.Infrastructure.Context;
using SGP.Shared.AppSettings;
using SGP.Shared.Extensions;
using System;
using System.Reflection;

namespace SGP.Infrastructure.Migrations
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IHealthChecksBuilder healthChecksBuilder)
        {
            Guard.Against.Null(services, nameof(services));
            Guard.Against.Null(healthChecksBuilder, nameof(healthChecksBuilder));

            services.AddDbContext<SgpContext>((serviceProvider, builder) =>
            {
                var connectionString = serviceProvider.GetConnectionString();
                var assemblyName = GetAssemblyName();

                builder.UseSqlServer(connectionString,
                    options => options.MigrationsAssembly(assemblyName));

                var environment = serviceProvider.GetRequiredService<IHostEnvironment>();
                if (environment.IsDevelopment())
                {
                    var loggerFactory = LoggerFactory.Create(logging => logging.AddConsole());
                    builder.UseLoggerFactory(loggerFactory);
                    builder.EnableDetailedErrors();
                    builder.EnableSensitiveDataLogging();
                }
            });

            // Verificador de saúde da base de dados.
            healthChecksBuilder.AddDbContextCheck<SgpContext>(
                tags: new[] { "database" },
                customTestQuery: (context, cancellationToken)
                    => context.Cities.AsNoTracking().AnyAsync(cancellationToken));

            return services;
        }

        private static string GetAssemblyName()
        {
            return Assembly.GetExecutingAssembly().GetName().Name;
        }

        private static string GetConnectionString(this IServiceProvider serviceProvider)
        {
            var connectionStringsOptions = serviceProvider.GetRequiredService<IOptions<ConnectionStrings>>();

            Guard.Against.Null(connectionStringsOptions, nameof(connectionStringsOptions));

            return connectionStringsOptions.Value.DefaultConnection;
        }
    }
}