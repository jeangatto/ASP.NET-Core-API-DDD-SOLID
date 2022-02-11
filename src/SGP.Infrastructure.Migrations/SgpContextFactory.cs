using System;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SGP.Infrastructure.Context;
using SGP.Shared.AppSettings;
using SGP.Shared.Extensions;

namespace SGP.Infrastructure.Migrations
{
    public class SgpContextFactory : IDesignTimeDbContextFactory<SgpContext>
    {
        private static readonly string ApiBasePath = Directory.GetCurrentDirectory() + "\\..\\SGP.PublicApi\\";
        private static readonly string AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;

        public SgpContext CreateDbContext(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(ApiBasePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName ?? Environments.Development}.json", true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetWithNonPublicProperties<ConnectionStrings>();

            var builder = new DbContextOptionsBuilder<SgpContext>()
                .UseSqlServer(connectionString.DefaultConnection, options => options.MigrationsAssembly(AssemblyName))
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();

            var loggerFactory = LoggerFactory.Create(logging => logging.AddConsole());

            return new SgpContext(builder.Options, loggerFactory);
        }
    }
}