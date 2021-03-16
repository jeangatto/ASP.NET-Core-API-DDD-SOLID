using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SGP.Application.Interfaces;
using SGP.Application.Requests;
using SGP.Application.Requests.CidadeRequests;
using SGP.Application.Requests.UsuarioRequests;
using SGP.Application.Responses;
using SGP.Application.Services;
using SGP.Domain.Repositories;
using SGP.Infrastructure.Context;
using SGP.Infrastructure.Repositories;
using SGP.Infrastructure.Services;
using SGP.Shared;
using SGP.Shared.Extensions;
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
                await context.EnsureSeedDataAsync();

                var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger(nameof(Program));

                var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
                mapper.ConfigurationProvider.AssertConfigurationIsValid();  // Validando os mapeamentos
                mapper.ConfigurationProvider.CompileMappings();             // Cacheando os mapeamentos

                var cidadeAppService = scope.ServiceProvider.GetRequiredService<ICidadeService>();

                var estados = await cidadeAppService.GetAllEstadosAsync();
                logger.LogInformation($"Total Estados: {estados.Count()}");

                var req0 = new GetAllByEstadoRequest("sp");
                var result0 = await cidadeAppService.GetAllAsync(req0);
                logger.LogInformation($"Total Cidades: {result0.Data.Count()}");

                var req1 = new GetByIbgeRequest("3530607");
                var result1 = await cidadeAppService.GetByIbgeAsync(req1);
                logger.LogInformation(result1.ToJson());

                var req2 = new GetByIbgeRequest("blá");
                var result2 = await cidadeAppService.GetByIbgeAsync(req2);
                logger.LogInformation(result2.ToJson());

                var usuarioAppService = scope.ServiceProvider.GetRequiredService<IUsuarioService>();

                var req3 = new AddUsuarioRequest
                {
                    Nome = "Gatto",
                    Email = "jean_gatto@hotmail.com",
                    Senha = "ab12345"
                };
                var result3 = await usuarioAppService.AddAsync(req3);
                logger.LogInformation(result3.ToJson());

                var req4 = new GetByIdRequest(result3.Data.Id);
                var usuario0 = await usuarioAppService.GetByIdAsync(req4);
                logger.LogInformation(usuario0.ToJson());

                var req5 = new GetByIdRequest(Guid.Empty);
                var usuario1 = await usuarioAppService.GetByIdAsync(req5);
                logger.LogInformation(usuario1.ToJson());
            }

            Console.WriteLine();
            Console.WriteLine("----------- TERMINOU -----------");
            Console.WriteLine();
            Console.WriteLine("Pressione qualquer tecla para fechar...");
            Console.ReadKey();
        }
    }
}