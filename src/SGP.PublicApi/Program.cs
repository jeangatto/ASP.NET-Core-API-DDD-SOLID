using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SGP.Infrastructure.Data.Context;
using SGP.Shared.AppSettings;

namespace SGP.PublicApi;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        await using var scope = host.Services.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<SgpContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();
        var connectionOptions = scope.ServiceProvider.GetRequiredService<IOptions<ConnectionOptions>>().Value;

        try
        {
            logger.LogInformation(
                "----- Connection: {Connection}, Collation: {Collation}",
                connectionOptions.DefaultConnection,
                connectionOptions.Collation);

            // if ((await context.Database.GetPendingMigrationsAsync()).Any())
            // {
            //     logger.LogInformation("----- Creating and migrating the database...");
            //     await context.Database.MigrateAsync();
            // }
            await context.Database.EnsureCreatedAsync();
            logger.LogInformation("----- Seeding database...");
            await context.EnsureSeedDataAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while populating the database");
            throw;
        }

        logger.LogInformation("----- Starting the application...");
        await host.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
        => Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                    .UseKestrel(options => options.AddServerHeader = false)
                    .UseStartup<Startup>();
            })
            .UseDefaultServiceProvider((context, options) =>
            {
                options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
                options.ValidateOnBuild = true;
            });
}