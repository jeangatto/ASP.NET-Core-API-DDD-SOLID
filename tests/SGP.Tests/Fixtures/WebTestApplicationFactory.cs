namespace SGP.Tests.Fixtures
{
    using Infrastructure.Context;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using PublicApi;
    using System;
    using System.Linq;

    public class WebTestApplicationFactory : WebApplicationFactory<Startup>
    {
        private const string ConnectionString = "DataSource=:memory:";
        private SqliteConnection _connection;

        public WebTestApplicationFactory() => Server.AllowSynchronousIO = true;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment(Environments.Development);

            builder.UseDefaultServiceProvider(options => options.ValidateScopes = true);

            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor =
                    services.FirstOrDefault(d => d.ServiceType == typeof(DbContextOptions<SgpContext>));
                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor);
                }

                _connection = new SqliteConnection(ConnectionString);
                _connection.Open();

                services.AddDbContext<SgpContext>(options => options
                    .UseSqlite(_connection)
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging());

                var serviceProvider = services.BuildServiceProvider(true);

                using (var scope = serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<SgpContext>();
                    var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
                    var logger = loggerFactory.CreateLogger<WebTestApplicationFactory>();

                    logger.LogInformation($"ConnectionString={context.Database.GetConnectionString()}");

                    try
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                        context.EnsureSeedDataAsync(loggerFactory).GetAwaiter().GetResult();
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