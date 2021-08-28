using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SGP.Infrastructure.Context;
using SGP.Tests.Fixtures;
using SGP.Tests.Mocks;
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
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                await context.EnsureSeedDataAsync(LoggerFactoryMock.Create());
            }
        }

        public Task DisposeAsync() => Task.CompletedTask;
    }
}