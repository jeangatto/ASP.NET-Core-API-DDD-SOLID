using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SGP.Infrastructure.Context;
using SGP.Tests.Fixtures;
using Xunit;

namespace SGP.Tests.IntegrationTests
{
    public abstract class IntegrationTestBase : IAsyncLifetime
    {
        protected readonly HttpClient Client;
        private readonly WebTestApplicationFactory _factory;

        protected IntegrationTestBase(WebTestApplicationFactory factory)
        {
            Client = factory.Server.CreateClient();
            _factory = factory;
        }

        public async Task InitializeAsync()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SgpContext>();
                var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<WebTestApplicationFactory>();

                try
                {
                    logger.LogInformation($"ConnectionString={context.Database.GetConnectionString()}");

                    await context.Database.EnsureDeletedAsync();
                    await context.Database.EnsureCreatedAsync();
                    await context.EnsureSeedDataAsync(loggerFactory);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Ocorreu um erro ao popular o banco de dados; {ex.Message}");
                    throw;
                }
            }
        }

        public Task DisposeAsync() => Task.CompletedTask;
    }
}