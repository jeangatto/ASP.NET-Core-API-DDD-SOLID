using System;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SGP.Infrastructure.Context;
using SGP.Shared.AppSettings;
using SGP.Shared.Extensions;

namespace SGP.Infrastructure.Migrations
{
    public class SgpContextFactory : IDesignTimeDbContextFactory<SgpContext>
    {
        // =======================================================
        // CLI-commands (https://www.entityframeworktutorial.net/efcore/cli-commands-for-ef-core-migration.aspx)
        // dotnet tool install --global dotnet-ef
        // dotnet tool update --global dotnet-ef
        // dotnet ef dbcontext info -p "./src/SGP.Infrastructure.Migrations"
        // dotnet ef dbcontext list -p "./src/SGP.Infrastructure.Migrations"
        // dotnet ef migrations list -p "./src/SGP.Infrastructure.Migrations"
        // dotnet ef migrations add "NomeMigracao" -p "./src/SGP.Infrastructure.Migrations"
        // =======================================================

        private static readonly string ApiBasePath = Directory.GetCurrentDirectory() + "\\..\\SGP.PublicApi\\";
        private static readonly string AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;

        public SgpContext CreateDbContext(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(ApiBasePath)
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile($"appsettings.{environmentName ?? Environments.Development}.json", true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetWithNonPublicProperties<ConnectionStrings>();

            var builder = new DbContextOptionsBuilder<SgpContext>()
                .UseSqlServer(connectionString.DefaultConnection, options => options.MigrationsAssembly(AssemblyName))
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();

            return new SgpContext(builder.Options);
        }
    }
}