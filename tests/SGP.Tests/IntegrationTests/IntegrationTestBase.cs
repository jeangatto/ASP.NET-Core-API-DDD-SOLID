using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SGP.Infrastructure.Data.Context;
using SGP.Tests.Fixtures;
using Xunit;
using Xunit.Abstractions;
using Xunit.Categories;

namespace SGP.Tests.IntegrationTests;

[IntegrationTest]
public abstract class IntegrationTestBase : IAsyncLifetime
{
    protected IntegrationTestBase(WebTestApplicationFactory factory, ITestOutputHelper outputHelper)
    {
        HttpClient = factory.CreateClient();
        OutputHelper = outputHelper;
        ServiceProvider = factory.Services;
    }

    protected HttpClient HttpClient { get; }
    protected IServiceProvider ServiceProvider { get; }
    protected ITestOutputHelper OutputHelper { get; }

    public async Task InitializeAsync()
    {
        await using var serviceScope = ServiceProvider.CreateAsyncScope();
        await using var context = serviceScope.ServiceProvider.GetRequiredService<SgpContext>();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
        await context.EnsureSeedDataAsync();
    }

    public Task DisposeAsync()
    {
        HttpClient.Dispose();
        return Task.CompletedTask;
    }
}