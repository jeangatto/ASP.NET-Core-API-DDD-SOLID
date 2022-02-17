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
        protected readonly ITestOutputHelper Output;

        protected IntegrationTestBase(WebTestApplicationFactory factory, ITestOutputHelper output)
        {
            HttpClient = factory.Server.CreateClient();
            _factory = factory;
            Output = output;
        }

        #endregion

        #region IAsyncLifetime

        public async Task InitializeAsync()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SgpContext>();
                Output.WriteLine($"Integration Test DbConnection: \"{context.Database.GetConnectionString()}\"");
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                await context.EnsureSeedDataAsync();
            }
        }

        public Task DisposeAsync() => Task.CompletedTask;

        #endregion
    }
}