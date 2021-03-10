using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SGP.Application.Interfaces;
using SGP.Application.Responses;
using SGP.Application.Services;
using SGP.Domain.Repositories;
using SGP.Infrastructure.Context;
using SGP.Infrastructure.Extensions;
using SGP.Infrastructure.Repositories;
using SGP.Infrastructure.Services;
using SGP.Shared.Interfaces;
using SGP.Shared.UnitOfWork;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SGP.ConsoleApp
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("----------- INICIOU -----------");
            Console.WriteLine();

            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = configurationBuilder.Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var services = new ServiceCollection();

            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddConsole();
            });

            //-----------------------IoC------------------------

            // AppServices
            services.AddScoped<IPaisService, PaisService>();

            // Repositories
            services.AddScoped<IPaisRepository, PaisRepository>();
            services.AddScoped<IEstadoRepository, EstadoRepository>();
            services.AddScoped<ICidadeRepository, CidadeRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            // InfraServices
            services.AddScoped<IHashService, HashService>();

            // UoW
            services.AddScoped<IUnitOfWork, UnitOfWork<SgpContext>>();

            // AutoMapper
            services.AddAutoMapper(typeof(PaisResponse).Assembly);

            // Entity Framework Core
            services.AddDbContext<SgpContext>(options => options.UseSqlServer(connectionString));

            //-------------------------------------------------

            var serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });

            using (var scope = serviceProvider.CreateScope())
            {
                var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger("Console");

                var context = scope.ServiceProvider.GetRequiredService<SgpContext>();
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                await context.EnsureSeedDataAsync();

                var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
                mapper.ConfigurationProvider.AssertConfigurationIsValid();
                mapper.ConfigurationProvider.CompileMappings();

                var paisService = scope.ServiceProvider.GetRequiredService<IPaisService>();
                var result = await paisService.GetAllAsync();
                logger.LogInformation(result.Data.ToJson());
            }

            Console.WriteLine();
            Console.WriteLine("----------- TERMINOU -----------");
            Console.WriteLine();
            Console.WriteLine("Pressione qualquer tecla para fechar...");
            Console.ReadKey();
        }
    }
}