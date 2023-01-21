using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SGP.Infrastructure.Data.Context;
using SGP.Shared.AppSettings;
using SGP.Shared.Extensions;

namespace SGP.PublicApi.Extensions;

internal static class DbContextExtensions
{
    internal static IServiceCollection AddSpgContext(this IServiceCollection services, IHealthChecksBuilder healthChecksBuilder)
    {
        services.AddDbContext<SgpContext>((serviceProvider, options) =>
        {
            var inMemoryOptions = serviceProvider.GetOptions<InMemoryOptions>();
            if (inMemoryOptions.Database)
            {
                options.UseInMemoryDatabase($"InMemory_{nameof(SgpContext)}");
            }
            else
            {
                var connections = serviceProvider.GetOptions<ConnectionStrings>();

                options.UseSqlServer(connections.Database, sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(Program).Assembly.GetName().Name);

                    // Configurando a resiliência da conexão: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 3);
                }).EnableServiceProviderCaching();
            }

            var logger = serviceProvider.GetRequiredService<ILogger<SgpContext>>();

            // Log tentativas de repetição
            options.LogTo(
                filter: (eventId, _) => eventId.Id == CoreEventId.ExecutionStrategyRetrying,
                logger: eventData =>
                {
                    if (eventData is not ExecutionStrategyEventData retryEventData)
                        return;

                    var exceptions = retryEventData.ExceptionsEncountered;
                    var count = exceptions.Count;
                    var delay = retryEventData.Delay;
                    var message = exceptions[^1].Message;
                    logger.LogWarning("----- Retry #{Count} with delay {Delay} due to error: {Message}", count, delay, message);
                });

            // Quando for ambiente de desenvolvimento será logado informações detalhadas.
            var environment = serviceProvider.GetRequiredService<IHostEnvironment>();
            if (environment.IsDevelopment())
                options.EnableDetailedErrors().EnableSensitiveDataLogging();
        });

        // Verificador de saúde da base de dados.
        healthChecksBuilder.AddDbContextCheck<SgpContext>(
            tags: new[] { "database" },
            customTestQuery: (context, cancellationToken) => context.Cidades.AsNoTracking().AnyAsync(cancellationToken));

        return services;
    }
}