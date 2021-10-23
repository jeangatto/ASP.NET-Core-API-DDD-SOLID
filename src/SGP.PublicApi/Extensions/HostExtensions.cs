using System;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SGP.Infrastructure.Context;

namespace SGP.PublicApi.Extensions
{
    public static class HostExtensions
    {
        internal static async Task MigrateDbContextAsync(this IHost host)
        {
            Guard.Against.Null(host, nameof(host));

            using (var scope = host.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
                var context = scope.ServiceProvider.GetRequiredService<SgpContext>();

                try
                {
                    logger.LogInformation($"ConnectionString: {context.Database.GetConnectionString()}");

                    if ((await context.Database.GetPendingMigrationsAsync()).Any())
                    {
                        // Aplica de maneira assíncrona quaisquer migrações pendentes do contexto.
                        // Criará o banco de dados, se ainda não existir.
                        await context.Database.MigrateAsync();
                    }

                    // Populando a base de dados com estados, cidades...
                    await context.EnsureSeedDataAsync();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Ocorreu um erro ao popular o banco de dados; {ex.Message}");
                    throw;
                }
            }
        }
    }
}