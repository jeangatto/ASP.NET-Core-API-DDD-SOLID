using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SGP.Application.Interfaces;
using SGP.Application.Requests.CidadeRequests;
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
using System.Linq;
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
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var configuration = configurationBuilder.Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var services = new ServiceCollection();

            //-----------------------IoC------------------------

            // Logging
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddConsole();
            });

            // Entity Framework Core
            services.AddDbContext<SgpContext>(options => options.UseSqlServer(connectionString));

            // UoW
            services.AddScoped<IUnitOfWork, UnitOfWork<SgpContext>>();

            // Repositories
            services.AddScoped<ICidadeRepository, CidadeRepository>();

            // InfraServices
            services.AddScoped<IHashService, HashService>();

            // AppServices
            services.AddScoped<ICidadeAppService, CidadeAppService>();

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
                await context.EnsureSeedDataAsync();

                var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger(nameof(Program));

                var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
                // Validando os mapeamentos
                mapper.ConfigurationProvider.AssertConfigurationIsValid();
                // Cacheando os mapeamentos.
                mapper.ConfigurationProvider.CompileMappings();

                var appService = scope.ServiceProvider.GetRequiredService<ICidadeAppService>();

                var estados = await appService.GetAllEstadosAsync();
                logger.LogInformation($"Total Estados: {estados.Count()}");

                var req0 = new GetAllByEstadoRequest("sp");
                var result0 = await appService.GetAllAsync(req0);
                logger.LogInformation($"Total Cidades: {result0.Data.Count()}");

                var req1 = new GetByIbgeRequest("3530607");
                var result1 = await appService.GetByIbgeAsync(req1);
                logger.LogInformation(result1.ToJson());

                var req2 = new GetByIbgeRequest("blá");
                var result2 = await appService.GetByIbgeAsync(req2);
                logger.LogInformation(result2.ToJson());
            }

            Console.WriteLine();
            Console.WriteLine("----------- TERMINOU -----------");
            Console.WriteLine();
            Console.WriteLine("Pressione qualquer tecla para fechar...");
            Console.ReadKey();
        }
    }
}