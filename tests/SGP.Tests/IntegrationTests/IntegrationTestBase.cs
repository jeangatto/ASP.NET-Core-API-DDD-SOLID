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
        protected readonly HttpClient HttpClient;
        protected readonly ITestOutputHelper OutputHelper;

        protected IntegrationTestBase(WebTestApplicationFactory factory, ITestOutputHelper outputHelper)
        {
            _factory = factory;
            HttpClient = factory.Server.CreateClient();
            OutputHelper = outputHelper;
        }

        #endregion

        #region IAsyncLifetime

        public async Task InitializeAsync()
        {
            using (var scope = _factory.Services.CreateScope())
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