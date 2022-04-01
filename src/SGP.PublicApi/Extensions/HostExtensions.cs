using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SGP.Infrastructure.Context;

namespace SGP.PublicApi.Extensions;

internal static class HostExtensions
{
    private const string LoggerCategoryName = "MigrateDbContext";

    internal static async Task MigrateDbContextAsync(this IHost host)
    {
        await using var scope = host.Services.CreateAsyncScope();
        var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger(LoggerCategoryName);
        var context = scope.ServiceProvider.GetRequiredService<SgpContext>();

        try
        {
            logger.LogInformation("Connection: {ConnectionString}", context.Database.GetConnectionString());

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
            logger.LogError(ex, "Ocorreu um erro ao popular o banco de dados");
            throw;
        }
    }
}