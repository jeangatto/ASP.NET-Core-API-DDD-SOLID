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
        public CidadesControllerTests(WebTestApplicationFactory factory, ITestOutputHelper outputHelper)
            : base(factory, outputHelper)
        {
        }

        [Fact]
        public async Task Devera_RetornarResultadoSucessoComCidades_AoObterPorUf()
        {
            // Arrange
            const int total = 645;
            const string uf = "SP";

            // Act
            var result
                = await HttpClient.SendAndDeserializeAsync<IEnumerable<CidadeResponse>>(
                    OutputHelper, $"/api/cidades/{uf}");

            // Assert
            result.Should().NotBeNullOrEmpty()
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
            var result
                = await HttpClient.SendAndDeserializeAsync<CidadeResponse>(OutputHelper, $"/api/cidades/{ibge}");

            // Assert
            result.Should().NotBeNull();
            result.Regiao.Should().NotBeNullOrWhiteSpace();
            result.Estado.Should().NotBeNullOrWhiteSpace();
            result.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2);
            result.Nome.Should().NotBeNullOrWhiteSpace();
            result.Ibge.Should().BePositive().And.Be(ibge);
        }
    }
}