using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using SGP.Application.Responses;
using SGP.Tests.Extensions;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.IntegrationTests.Controllers;

[IntegrationTest]
public class CidadesControllerTests
{
    [Fact]
    public async Task Devera_RetornarResultadoSucessoComCidades_AoObterPorUf()
    {
        // Arrange
        await using var webApplicationFactory = CreateWebApplication();
        using var httpClient = webApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        const int total = 645;
        const string uf = "SP";
        const string endPoint = $"/api/cidades/{uf}";

        // Act
        var act = await httpClient.GetAsync<IEnumerable<CidadeResponse>>(endPoint);

        // Assert
        act.Should().NotBeNullOrEmpty()
            .And.OnlyHaveUniqueItems()
            .And.HaveCount(total)
            .And.Subject.ForEach(response =>
            {
                response.Regiao.Should().NotBeNullOrWhiteSpace();
                response.Estado.Should().NotBeNullOrWhiteSpace();
                response.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2).And.Be(uf);
                response.Nome.Should().NotBeNullOrWhiteSpace();
                response.Ibge.Should().BePositive();
            });
    }

    [Fact]
    public async Task Devera_RetornarResultadoSucessoComCidade_AoObterPorIbge()
    {
        // Arrange
        await using var webApplicationFactory = CreateWebApplication();
        using var httpClient = webApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        const int ibge = 3557105;
        var endpoint = $"/api/cidades/{ibge}";

        // Act
        var act = await httpClient.GetAsync<CidadeResponse>(endpoint);

        // Assert
        act.Should().NotBeNull();
        act.Regiao.Should().NotBeNullOrWhiteSpace();
        act.Estado.Should().NotBeNullOrWhiteSpace();
        act.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2);
        act.Nome.Should().NotBeNullOrWhiteSpace();
        act.Ibge.Should().BePositive().And.Be(ibge);
    }

    private static WebApplicationFactory<Program> CreateWebApplication() =>
        new WebApplicationFactory<Program>()
            .WithWebHostBuilder(hostBuilder => hostBuilder.ConfigureLogging(logging => logging.ClearProviders()));
}