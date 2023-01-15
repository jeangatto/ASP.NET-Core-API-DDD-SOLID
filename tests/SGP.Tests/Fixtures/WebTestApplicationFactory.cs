using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using SGP.Infrastructure.Data.Context;
using Environments = SGP.Tests.Constants.Environments;

namespace SGP.Tests.Fixtures;

public class WebTestApplicationFactory : WebApplicationFactory<PublicApi.Program>
{
    private SqliteConnection _connection;

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

                var serviceProvider = services.BuildServiceProvider(true);
                using var serviceScope = serviceProvider.CreateScope();
                using var context = serviceScope.ServiceProvider.GetRequiredService<SgpContext>();
                context.Database.EnsureCreated();
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