using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SGP.Infrastructure.Context;
using SGP.Tests.Fixtures;
using Xunit;

namespace SGP.Tests.IntegrationTests
{
    public abstract class IntegrationTestBase : IAsyncLifetime
    {
        #region Constructor

        private readonly WebTestApplicationFactory _factory;

        protected IntegrationTestBase(WebTestApplicationFactory factory)
        {
            HttpClient = factory.Server.CreateClient();
            _factory = factory;
        }

        #endregion

        protected HttpClient HttpClient { get; }

        #region IAsyncLifetime

        public async Task InitializeAsync()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SgpContext>();
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                await context.EnsureSeedDataAsync();
            }
        }

        public Task DisposeAsync() => Task.CompletedTask;

        #endregion
    }
}