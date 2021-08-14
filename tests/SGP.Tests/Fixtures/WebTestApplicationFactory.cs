using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SGP.Infrastructure.Context;
using SGP.PublicApi;
using SGP.Tests.Constants;

namespace SGP.Tests.Fixtures
{
    public class WebTestApplicationFactory : WebApplicationFactory<Startup>
    {
        private const string ConnectionString = "DataSource=:memory:";
        private SqliteConnection _connection;

        public WebTestApplicationFactory() => Server.AllowSynchronousIO = true;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment(Environments.Test);

            builder.UseDefaultServiceProvider(options => options.ValidateScopes = true);

            builder.ConfigureServices(services =>
            {
                var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(DbContextOptions<SgpContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                _connection = new SqliteConnection(ConnectionString);
                _connection.Open();

                services.AddDbContext<SgpContext>(options => options
                    .UseSqlite(_connection)
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging());

                using (var scope = services.BuildServiceProvider(true).CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<SgpContext>();
                    var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
                    var logger = loggerFactory.CreateLogger<WebTestApplicationFactory>();

                    logger.LogInformation($"ConnectionString={context.Database.GetConnectionString()}");

                    try
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                        var rowsAffected = context.EnsureSeedDataAsync(loggerFactory).GetAwaiter().GetResult();
                        logger.LogInformation($"Total de linhas populadas: {rowsAffected}");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"Ocorreu um erro ao popular o banco de dados; {ex.Message}");
                        throw;
                    }
                }
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _connection?.Dispose();
                _connection = null;
            }

            base.Dispose(disposing);
        }
    }
}