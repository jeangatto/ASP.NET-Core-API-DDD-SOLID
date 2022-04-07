using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SGP.Infrastructure.Context;

namespace SGP.PublicApi.Extensions;

internal static class HostExtensions
{
    internal static async Task MigrateDbContextAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<SgpContext>();

        try
        {
            app.Logger.LogInformation("Connection: {ConnectionString}", context.Database.GetConnectionString());

            if ((await context.Database.GetPendingMigrationsAsync()).Any())
                await context.Database.MigrateAsync();

            await context.EnsureSeedDataAsync();
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "An error occurred while populating the database");
            throw;
        }
    }
}