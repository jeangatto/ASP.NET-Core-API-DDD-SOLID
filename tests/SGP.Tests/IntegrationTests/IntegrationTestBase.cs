using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SGP.Infrastructure.Context;
using SGP.Tests.Fixtures;
using Xunit;
using Xunit.Abstractions;
using Xunit.Categories;

namespace SGP.Tests.IntegrationTests
{
    [IntegrationTest]
    public abstract class IntegrationTestBase : IAsyncLifetime
    {
        #region Constructor

        private readonly WebTestApplicationFactory _factory;

        protected IntegrationTestBase(WebTestApplicationFactory factory, ITestOutputHelper outputHelper)
        {
            _factory = factory;
            HttpClient = factory.Server.CreateClient();
            OutputHelper = outputHelper;
        }

        #endregion

        protected HttpClient HttpClient { get; }
        protected ITestOutputHelper OutputHelper { get; }

        #region IAsyncLifetime

        public async Task InitializeAsync()
        {
            using (var scope = _factory.Services.CreateAsyncScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SgpContext>();
                OutputHelper.WriteLine($"Integration Test DbConnection: \"{context.Database.GetConnectionString()}\"");
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                await context.EnsureSeedDataAsync();
            }
        }

        public Task DisposeAsync()
        {
            HttpClient.Dispose();
            return Task.CompletedTask;
        }

        #endregion
    }
}