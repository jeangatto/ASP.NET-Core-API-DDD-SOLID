using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using SGP.Infrastructure.Context;
using SGP.PublicApi;
using Environments = SGP.Tests.Constants.Environments;

namespace SGP.Tests.Fixtures
{
    public class WebTestApplicationFactory : WebApplicationFactory<Startup>
    {
        private SqliteConnection _connection;

        public WebTestApplicationFactory()
            => Server.AllowSynchronousIO = true;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
            => builder
                .UseEnvironment(Environments.Testing)
                .UseDefaultServiceProvider(options => options.ValidateScopes = true)
                .ConfigureTestServices(services => services.RemoveAll<IHostedService>())
                .ConfigureServices(services =>
                {
                    var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(DbContextOptions<SgpContext>));
                    if (descriptor != null)
                        services.Remove(descriptor);

                    _connection = new SqliteConnection(ConnectionString.Sqlite);
                    _connection.Open();

                    services.AddDbContext<SgpContext>(options => options.UseSqlite(_connection));
                });

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