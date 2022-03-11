using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using SGP.Application.Responses;
using SGP.Tests.Extensions;
using SGP.Tests.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace SGP.Tests.IntegrationTests.Controllers.v1
{
    public class CidadesControllerTests : IntegrationTestBase, IClassFixture<WebTestApplicationFactory>
    {
        public CidadesControllerTests(WebTestApplicationFactory factory, ITestOutputHelper outputHelper) : base(factory, outputHelper)
        {
        }

        [Fact]
        public async Task Devera_RetornarResultadoSucessoComCidades_AoObterPorUf()
        {
            // Arrange
            const int total = 645;
            const string uf = "SP";

            // Act
            var response = await HttpClient.GetAsync($"/api/cidades/{uf}");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var data = await response.Content.GetApiDataAsync<IEnumerable<CidadeResponse>>();
            data.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.HaveCount(total)
                .And.Subject.ForEach(c =>
                {
                    c.Regiao.Should().NotBeNullOrWhiteSpace();
                    c.Estado.Should().NotBeNullOrWhiteSpace();
                    c.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2).And.Be(uf);
                    c.Nome.Should().NotBeNullOrWhiteSpace();
                    c.Ibge.Should().BePositive();
                });
        }

        [Fact]
        public async Task Devera_RetornarResultadoSucessoComCidade_AoObterPorIbge()
        {
            // Arrange
            const int ibge = 3557105;

            // Act
            var response = await HttpClient.GetAsync($"/api/cidades/{ibge}");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var data = await response.Content.GetApiDataAsync<CidadeResponse>();
            data.Should().NotBeNull();
            data.Regiao.Should().NotBeNullOrWhiteSpace();
            data.Estado.Should().NotBeNullOrWhiteSpace();
            data.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2);
            data.Nome.Should().NotBeNullOrWhiteSpace();
            data.Ibge.Should().BePositive().And.Be(ibge);
        }
    }
}