using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SGP.Infrastructure.Data.Context;
using SGP.Shared.AppSettings;
using SGP.Shared.Extensions;

namespace SGP.PublicApi.Extensions;

[ExcludeFromCodeCoverage]
internal static class DbContextExtensions
{
    private const int DbMaxRetryCount = 3;
    private const int DbCommandTimeout = 35;
    private const string DbInMemoryName = $"Db-InMemory-{nameof(SgpContext)}";
    private const string DbMigrationAssemblyName = "SGP.PublicApi";

    private static readonly string[] DbRelationalTags = { "database", "ef-core", "sql-server", "relational" };

    internal static IServiceCollection AddSpgContext(this IServiceCollection services, IHealthChecksBuilder healthChecksBuilder)
    {
        services.AddDbContext<SgpContext>((serviceProvider, optionsBuilder) =>
        {
            var inMemoryOptions = serviceProvider.GetOptions<InMemoryOptions>();
            if (inMemoryOptions.Database)
            {
                optionsBuilder.UseInMemoryDatabase(DbInMemoryName + $"-{Guid.NewGuid()}");
            }
            else
            {
                var connections = serviceProvider.GetOptions<ConnectionStrings>();

                optionsBuilder.UseSqlServer(connections.Database, sqlServerOptions =>
                {
                    sqlServerOptions.MigrationsAssembly(DbMigrationAssemblyName);
                    sqlServerOptions.EnableRetryOnFailure(DbMaxRetryCount);
                    sqlServerOptions.CommandTimeout(DbCommandTimeout);
                });
            }

            var logger = serviceProvider.GetRequiredService<ILogger<SgpContext>>();

            // Log tentativas de repetição
            optionsBuilder.LogTo(
                (eventId, _) => eventId.Id == CoreEventId.ExecutionStrategyRetrying,
                eventData =>
                {
                    if (eventData is not ExecutionStrategyEventData retryEventData)
                        return;

                    var exceptions = retryEventData.ExceptionsEncountered;

                    logger.LogWarning(
                        "----- Retry #{Count} with delay {Delay} due to error: {Message}",
                        exceptions.Count,
                        retryEventData.Delay,
                        exceptions[^1].Message);
                });

            var environment = serviceProvider.GetRequiredService<IHostEnvironment>();
            if (!environment.IsProduction())
            {
                optionsBuilder.EnableDetailedErrors();
                optionsBuilder.EnableSensitiveDataLogging();
            }
        });

        // Verificador de saúde da base de dados.
        healthChecksBuilder.AddDbContextCheck<SgpContext>(
            tags: DbRelationalTags,
            customTestQuery: (context, cancellationToken) =>
                context.Cidades.AsNoTracking().AnyAsync(cancellationToken));

        return services;
    }
}
