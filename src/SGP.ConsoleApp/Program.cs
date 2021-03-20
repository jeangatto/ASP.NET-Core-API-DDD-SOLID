using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SGP.Application.Interfaces;
using SGP.Application.Responses;
using SGP.Application.Services;
using SGP.Domain.Repositories;
using SGP.Infrastructure.Data.Context;
using SGP.Infrastructure.Data.Repositories;
using SGP.Infrastructure.Services;
using SGP.Shared;
using SGP.Shared.Interfaces;
using SGP.Shared.UnitOfWork;
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

            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var configuration = configurationBuilder.Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var services = new ServiceCollection();

            //-----------------------IoC------------------------

            services.AddSingleton<IConfiguration>(configuration);

            // Logging
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddConsole();
            });

            // Entity Framework Core
            services.AddDbContext<SgpContext>(options => options.UseSqlServer(connectionString));

            // Shared
            services.AddScoped<IDateTimeService, DateTimeService>();
            services.AddScoped<IUnitOfWork, UnitOfWork<SgpContext>>();

            // Repositories
            services.AddScoped<ICidadeRepository, CidadeRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            // InfraServices
            services.AddScoped<IHashService, HashService>();

            // AppServices
            services.AddScoped<ICidadeService, CidadeService>();
            services.AddScoped<IUsuarioService, UsuarioService>();

            // AutoMapper
            services.AddAutoMapper(typeof(CidadeResponse).Assembly);

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