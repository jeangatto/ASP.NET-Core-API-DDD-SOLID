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
    internal static async Task MigrateDbContextAsync(this IHost host)
    {
        await using var scope = host.Services.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<SgpContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();

        try
        {
            logger.LogInformation("Connection: {ConnectionString}", context.Database.GetConnectionString());

            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                logger.LogInformation("Creating and migrating the database...");
                await context.Database.MigrateAsync();
            }

            logger.LogInformation("Seeding database...");
            await context.EnsureSeedDataAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while populating the database");
            throw;
        }
    }
}