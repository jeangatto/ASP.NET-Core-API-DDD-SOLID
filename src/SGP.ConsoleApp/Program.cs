using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SGP.Application;
using SGP.Infrastructure;
using SGP.Infrastructure.Context;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SGP.ConsoleApp
{
    public static class Program
    {
        public static async Task Main()
        {
            Console.WriteLine("----------- INICIOU -----------");
            Console.WriteLine();

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            //-----------------------IoC------------------------
            var services = new ServiceCollection();

            services.AddSingleton(configuration);

            // Logging
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddConsole();
            });

            // EF Core Context
            services.AddDbContext<SgpContext>(options => options.UseSqlServer(connectionString));
            services.ConfigureAppSettings(configuration);
            services.AddInfrastructure();
            services.AddApplication();

            //-------------------------------------------------

            var serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });

            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SgpContext>();
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();

                var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
                await context.EnsureSeedDataAsync(loggerFactory);
            }

            Console.WriteLine();
            Console.WriteLine("----------- TERMINOU -----------");
            Console.WriteLine();
            Console.WriteLine("Pressione qualquer tecla para fechar...");
            Console.ReadKey();
        }
    }
}