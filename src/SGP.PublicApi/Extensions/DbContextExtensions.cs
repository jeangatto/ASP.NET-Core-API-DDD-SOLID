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
    internal static IServiceCollection AddSpgContext(this IServiceCollection services, IHealthChecksBuilder healthChecksBuilder)
    {
        services.AddDbContext<SgpContext>((serviceProvider, dbBuilderOptions) =>
        {
            var inMemoryOptions = serviceProvider.GetOptions<InMemoryOptions>();
            if (inMemoryOptions.Database)
            {
                dbBuilderOptions.UseInMemoryDatabase($"IN_MEMORY_{nameof(SgpContext)}");
            }
            else
            {
                var connections = serviceProvider.GetOptions<ConnectionStrings>();

                dbBuilderOptions.UseSqlServer(connections.Database, sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(Program).Assembly.GetName().Name);
                    sqlOptions.EnableRetryOnFailure(3);
                });
            }

            var logger = serviceProvider.GetRequiredService<ILogger<SgpContext>>();

            // Log tentativas de repetição
            dbBuilderOptions.LogTo(
                (eventId, _) => eventId.Id == CoreEventId.ExecutionStrategyRetrying,
                eventData =>
                {
                    if (eventData is not ExecutionStrategyEventData retryEventData)
                        return;

                    var exceptions = retryEventData.ExceptionsEncountered;
                    var count = exceptions.Count;
                    var delay = retryEventData.Delay;
                    var message = exceptions[^1].Message;

                    logger.LogWarning(
                        "----- Retry #{Count} with delay {Delay} due to error: {Message}",
                        count,
                        delay,
                        message);
                });

            // Quando for ambiente de desenvolvimento será logado informações detalhadas.
            var environment = serviceProvider.GetRequiredService<IHostEnvironment>();
            if (environment.IsDevelopment())
            {
                dbBuilderOptions
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging();
            }
        });

        // Verificador de saúde da base de dados.
        healthChecksBuilder.AddDbContextCheck<SgpContext>(
            tags: new[] { "database", "dbcontext" },
            customTestQuery: (context, cancellationToken) =>
                context.Cidades.AsNoTracking().AnyAsync(cancellationToken));

        return services;
    }
}
