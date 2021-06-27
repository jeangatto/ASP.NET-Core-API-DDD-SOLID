using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SGP.Infrastructure.Context;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SGP.PublicApi
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger(nameof(Program));
                var context = serviceProvider.GetRequiredService<SgpContext>();

                try
                {
                    logger.LogInformation($"ConnectionString={context.Database.GetConnectionString()}");

                    if ((await context.Database.GetPendingMigrationsAsync()).Any())
                    {
                        // Aplica de maneira assíncrona quaisquer migrações pendentes do contexto.
                        // Criará o banco de dados, se ainda não existir.
                        await context.Database.MigrateAsync();
                    }

                    await context.EnsureSeedDataAsync(loggerFactory);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Ocorreu um erro ao popular o banco de dados.");
                    throw;
                }
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                    webBuilder.UseStartup<Startup>();
                })
                .UseDefaultServiceProvider((context, options) =>
                {
                    options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
                    options.ValidateOnBuild = true;
                });
    }
}
