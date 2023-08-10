using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using SGP.Application.Responses;
using SGP.Tests.Extensions;
using SGP.Tests.Fixtures;
using Xunit;

namespace SGP.Tests.IntegrationTests.Controllers;

public class CidadesControllerTests : IntegrationTestBase, IClassFixture<WebTestApplicationFactory>
{
    public CidadesControllerTests(WebTestApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Devera_RetornarResultadoSucessoComCidades_AoObterPorUf()
    {
        // Arrange
        const int total = 645;
        const string uf = "SP";
        const string endPoint = $"/api/cidades/{uf}";

        // Act
        var act = await HttpClient.GetAsync<IEnumerable<CidadeResponse>>(endPoint);

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
        const int ibge = 3557105;
        var endPoint = $"/api/cidades/{ibge}";

        // Act
        var act = await HttpClient.GetAsync<CidadeResponse>(endPoint);

        // Assert
        act.Should().NotBeNull();
        act.Regiao.Should().NotBeNullOrWhiteSpace();
        act.Estado.Should().NotBeNullOrWhiteSpace();
        act.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2);
        act.Nome.Should().NotBeNullOrWhiteSpace();
        act.Ibge.Should().BePositive().And.Be(ibge);
    }
}