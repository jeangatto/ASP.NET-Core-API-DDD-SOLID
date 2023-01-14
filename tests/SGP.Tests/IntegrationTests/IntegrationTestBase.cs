using System;
using System.Net.Http;
using SGP.Tests.Fixtures;
using Xunit.Categories;

namespace SGP.Tests.IntegrationTests;

[IntegrationTest]
public abstract class IntegrationTestBase
{
    protected IntegrationTestBase(WebTestApplicationFactory factory)
    {
        HttpClient = factory.CreateClient();
        ServiceProvider = factory.Services;
    }

    protected HttpClient HttpClient { get; }
    protected IServiceProvider ServiceProvider { get; }
}