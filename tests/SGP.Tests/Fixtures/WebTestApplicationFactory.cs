using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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