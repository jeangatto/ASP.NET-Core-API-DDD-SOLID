using FluentAssertions;
using GraphQL;
using GraphQL.Client.Abstractions;
using SGP.Application.Responses;
using SGP.PublicApi.GraphQL.Constants;
using SGP.SharedTests;
using SGP.SharedTests.Extensions;
using SGP.SharedTests.Fixtures;
using SGP.SharedTests.GraphQL;
using SGP.SharedTests.TestDatas;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace SGP.IntegrationTests.GraphQL
{
    [Category(TestCategories.GraphQL)]
    public class CidadeSchemaTests : CidadeTestData, IClassFixture<WebTestApplicationFactory>
    {
        private readonly IGraphQLClient _graphClient;

        public CidadeSchemaTests(WebTestApplicationFactory factory)
        {
            var httpClient = factory.Server.CreateClient();
            _graphClient = GraphQLClient.Create(httpClient, GraphQLApiEndpoints.Cidades);
        }

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

            var request = new GraphQLRequest { Query = "{" + query.Build() + "}" };

            // Act
            var response = await _graphClient.SendQueryAsync<CidadesPorEstadoResponse>(request);

            // Assert
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.CidadesPorEstado.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.HaveCount(totalEsperado)
                .And.Subject.ForEach(cidade =>
                {
                    cidade.Regiao.Should().NotBeNullOrWhiteSpace();
                    cidade.Estado.Should().NotBeNullOrWhiteSpace();
                    cidade.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2).And.Be(uf);
                    cidade.Nome.Should().NotBeNullOrWhiteSpace();
                    cidade.Ibge.Should().BePositive();
                });
        }

        [Theory]
        [ClassData(typeof(FiltrarPorIbgeData))]
        public async Task Devera_RetornarResultadoSucessoComCidade_QuandoObterPorIbge(int ibge, string cidadeEsperada, string ufEsperada, string regiaoEsperada)
        {
            // Arrange
            var query = new QueryCamelCase<CidadeResponse>(QueryNames.CidadePorIBGE)
                .AddArguments(new { ibge })
                .AddField(c => c.Regiao)
                .AddField(c => c.Estado)
                .AddField(c => c.Uf)
                .AddField(c => c.Nome)
                .AddField(c => c.Ibge);

            var request = new GraphQLRequest { Query = "{" + query.Build() + "}" };

            // Act
            var response = await _graphClient.SendQueryAsync<CidadePorIbgeResponse>(request);

            // Assert
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.CidadePorIBGE.Regiao.Should().NotBeNullOrWhiteSpace()
                .And.Be(regiaoEsperada);
            response.Data.CidadePorIBGE.Estado.Should().NotBeNullOrWhiteSpace();
            response.Data.CidadePorIBGE.Uf.Should().NotBeNullOrWhiteSpace()
                .And.HaveLength(2).And.Be(ufEsperada);
            response.Data.CidadePorIBGE.Nome.Should().NotBeNullOrWhiteSpace().And.Be(cidadeEsperada);
            response.Data.CidadePorIBGE.Ibge.Should().BePositive().And.Be(ibge);
        }

        private class CidadePorIbgeResponse
        {
            public CidadePorIbgeResponse(CidadeResponse cidadePorIBGE)
                => CidadePorIBGE = cidadePorIBGE;

            public CidadeResponse CidadePorIBGE { get; }
        }

        private class CidadesPorEstadoResponse
        {
            public CidadesPorEstadoResponse(IEnumerable<CidadeResponse> cidadesPorEstado)
                => CidadesPorEstado = cidadesPorEstado;

            public IEnumerable<CidadeResponse> CidadesPorEstado { get; }
        }
    }
}