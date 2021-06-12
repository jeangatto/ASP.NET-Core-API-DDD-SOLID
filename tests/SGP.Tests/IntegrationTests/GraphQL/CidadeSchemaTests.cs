using FluentAssertions;
using GraphQL;
using GraphQL.Client.Abstractions;
using SGP.Application.Responses;
using SGP.PublicApi.GraphQL.Constants;
using SGP.Tests.Factories;
using SGP.Tests.GraphQL;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.IntegrationTests.GraphQL
{
    [Category(TestCategories.GraphQL)]
    public class CidadeSchemaTests : IClassFixture<WebTestApplicationFactory>
    {
        private readonly IGraphQLClient _graphClient;

        public CidadeSchemaTests(WebTestApplicationFactory factory)
        {
            var httpClient = factory.Server.CreateClient();
            _graphClient = GraphQLClient.Create(httpClient, GraphQLApiEndpoints.Cidades);
        }

        [Fact]
        public async Task Devera_RetornarCidades_QuandoObterPorUf()
        {
            // Arrange
            var query = new QueryCamelCase<CidadeResponse>(QueryNames.CidadesPorEstado)
                .AddArguments(new { uf = "SP" })
                .AddField(c => c.Regiao)
                .AddField(c => c.Estado)
                .AddField(c => c.Uf)
                .AddField(c => c.Nome)
                .AddField(c => c.Ibge);

            var request = new GraphQLRequest { Query = "{" + query.Build() + "}" };

            // Act
            var response = await _graphClient.SendQueryAsync<CidadeQueryResponse>(request);

            // Assert
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull();
            response.Data.CidadesPorEstado.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.HaveCount(645);
        }

        private class CidadeQueryResponse
        {
            public CidadeQueryResponse(IEnumerable<CidadeResponse> cidadesPorEstado)
                => CidadesPorEstado = cidadesPorEstado;

            public IEnumerable<CidadeResponse> CidadesPorEstado { get; }
        }
    }
}