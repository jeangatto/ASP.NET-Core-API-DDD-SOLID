using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SGP.Application.Interfaces;
using SGP.Application.Requests.AuthRequests;
using SGP.Application.Requests.UsuarioRequests;
using SGP.Application.Responses;
using SGP.Application.Services;
using SGP.Domain.Repositories;
using SGP.Infrastructure.Data.Context;
using SGP.Infrastructure.Data.Repositories;
using SGP.Infrastructure.Services;
using SGP.Shared.AppSettings;
using SGP.Shared.Extensions;
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

            // AppSettings
            static void configureBinder(BinderOptions options) => options.BindNonPublicProperties = true;
            services.Configure<AuthConfig>(configuration.GetSection(nameof(AuthConfig)), configureBinder);
            services.Configure<JwtConfig>(configuration.GetSection(nameof(JwtConfig)), configureBinder);
            services.Configure<ConnectionStrings>(configuration.GetSection(nameof(ConnectionStrings)), configureBinder);

            // Logging
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddConsole();
            });

            // Domain - Shared
            services.AddScoped<IDateTimeService, LocalDateTimeService>();
            services.AddScoped<IUnitOfWork, UnitOfWork<SgpContext>>();

            // Infrastructure - EF Core Context
            services.AddDbContext<SgpContext>(options => options.UseSqlServer(connectionString));

            // Infrastructure - Repositories
            services.AddScoped<ICidadeRepository, CidadeRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            // Infrastructure - Services
            services.AddScoped<IHashService, HashService>();

            // Application - Services
            services.AddScoped<IAuthService, AuthService>();
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

                var usuarioService = scope.ServiceProvider.GetService<IUsuarioService>();
                await usuarioService.AddAsync(new AddUsuarioRequest
                {
                    Nome = "Gerência",
                    Email = "gerencia@hotmail.com",
                    Senha = "1234"
                });

                var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
                var result = await authService.AuthenticateAsync(new AuthRequest("gerencia@hotmail.com", "1234"));
                Console.WriteLine(result.ToJson());
            }

            Console.WriteLine();
            Console.WriteLine("----------- TERMINOU -----------");
            Console.WriteLine();
            Console.WriteLine("Pressione qualquer tecla para fechar...");
            Console.ReadKey();
        }
    }
}