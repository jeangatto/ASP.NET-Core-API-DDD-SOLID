using FluentAssertions;
using SGP.Application.Responses;
using SGP.PublicApi.GraphQL.Constants;
using SGP.SharedTests;
using SGP.SharedTests.Extensions;
using SGP.SharedTests.Fixtures;
using SGP.SharedTests.GraphQL;
using SGP.SharedTests.TestDatas;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace SGP.IntegrationTests.GraphQL
{
    [Category(TestCategories.GraphQL)]
    public class CidadeSchemaTests : CidadeTestData, IClassFixture<WebTestApplicationFactory>
    {
        private readonly HttpClient _client;

        public CidadeSchemaTests(WebTestApplicationFactory factory)
            => _client = factory.Server.CreateClient();

        [Theory]
        [ClassData(typeof(FiltrarPorUfData))]
        public async Task Devera_RetornarCidades_QuandoObterPorUf(string uf, int totalEsperado)
        {
            // Arrange
            var query = new QueryCamelCase<CidadeResponse>(QueryNames.CidadesPorEstado)
                .AddArguments(new { uf })
                .AddField(c => c.Regiao)
                .AddField(c => c.Estado)
                .AddField(c => c.Uf)
                .AddField(c => c.Nome)
                .AddField(c => c.Ibge);

            var request = new GraphQLRequest(query);

            // Act
            var response = await _client.SendAsync(GraphQLApiEndpoints.Cidades, request);

            // Assert
            response.EnsureSuccessStatusCode();

            var data = await response.Content.GetGraphQLDataAsync<IEnumerable<CidadeResponse>>(QueryNames.CidadesPorEstado);
            data.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.HaveCount(totalEsperado)
                .And.Subject.ForEach(c =>
                {
                    c.Regiao.Should().NotBeNullOrWhiteSpace();
                    c.Estado.Should().NotBeNullOrWhiteSpace();
                    c.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2).And.Be(uf);
                    c.Nome.Should().NotBeNullOrWhiteSpace();
                    c.Ibge.Should().BePositive();
                });
        }

        [Theory]
        [ClassData(typeof(FiltrarPorIbgeData))]
        public async Task Devera_RetornarResultadoSucessoComCidade_QuandoObterPorIbge(int ibge,
            string cidadeEsperada, string ufEsperada, string regiaoEsperada)
        {
            // Arrange
            var query = new QueryCamelCase<CidadeResponse>(QueryNames.CidadePorIbge)
                .AddArguments(new { ibge })
                .AddField(c => c.Regiao)
                .AddField(c => c.Estado)
                .AddField(c => c.Uf)
                .AddField(c => c.Nome)
                .AddField(c => c.Ibge);

            var request = new GraphQLRequest { Query = "{" + query.Build() + "}" };

            // Act
            var response = await _client.SendAsync(GraphQLApiEndpoints.Cidades, request);

            // Assert
            response.EnsureSuccessStatusCode();

            var data = await response.Content.GetGraphQLDataAsync<CidadeResponse>(QueryNames.CidadePorIbge);
            data.Should().NotBeNull();
            data.Regiao.Should().NotBeNullOrWhiteSpace().And.Be(regiaoEsperada);
            data.Estado.Should().NotBeNullOrWhiteSpace();
            data.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2).And.Be(ufEsperada);
            data.Nome.Should().NotBeNullOrWhiteSpace().And.Be(cidadeEsperada);
            data.Ibge.Should().BePositive().And.Be(ibge);
        }
    }
}