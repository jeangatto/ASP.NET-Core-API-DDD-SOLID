using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SGP.Domain.Repositories;
using SGP.Infrastructure.Context;
using SGP.Infrastructure.Repositories;
using SGP.Infrastructure.Services;
using SGP.Shared.Interfaces;
using System;
using System.Threading.Tasks;

namespace SGP.ConsoleApp
{
    public static class Program
    {
        // LocalDb
        // private const string ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=SGPContexto;Trusted_Connection=True;MultipleActiveResultSets=true";
        private const string ConnectionString = "Data Source=GATTO;Initial Catalog=SGPContexto;Integrated Security=True;MultipleActiveResultSets=true";

        public static async Task Main(string[] args)
        {
            Console.WriteLine("----------- ÍNICIOU ----------- ");

            var services = new ServiceCollection();

            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddConsole();
            });

            services.AddScoped<IHashService, HashService>();
            services.AddScoped<ICidadeRepository, CidadeRepository>();
            services.AddScoped<IEstadoRepository, EstadoRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            // Entity Framework Context
            services.AddDbContext<SGPContext>(builder => builder.UseSqlServer(ConnectionString));

            var serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });

            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SGPContext>();
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
            }

            Console.WriteLine("----------- TERMINOU ----------- ");
            Console.ReadKey();
        }
    }
}