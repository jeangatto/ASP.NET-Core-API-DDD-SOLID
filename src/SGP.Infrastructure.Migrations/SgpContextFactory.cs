using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SGP.Infrastructure.Context;
using SGP.Shared.AppSettings;
using System;
using System.IO;

namespace SGP.Infrastructure.Migrations
{
    public class SgpContextFactory : IDesignTimeDbContextFactory<SgpContext>
    {
        public SgpContext CreateDbContext(string[] args)
        {
            // Obtendo o tipo de ambiente.
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Construindo a configuração.
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../SGP.PublicApi"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            // Obtendo a string de conexão.
            var connectionString = configuration.GetConnectionString(
                nameof(ConnectionStrings.DefaultConnection));

            var optionsBuilder = new DbContextOptionsBuilder<SgpContext>()
                .UseSqlServer(connectionString, sqlServerBuilder
                    => sqlServerBuilder.MigrationsAssembly(MigrationsAssembly.Name));

            // Configurando para exibir os errados mais detalhados.
            if (environment == Environments.Development)
            {
                optionsBuilder.EnableDetailedErrors();
                optionsBuilder.EnableSensitiveDataLogging();
            }

            return new SgpContext(optionsBuilder.Options);
        }
    }
}