using FluentAssertions;
using SGP.Application.Responses;
using SGP.PublicApi.GraphQL.Constants;
using SGP.SharedTests.Constants;
using SGP.SharedTests.Extensions;
using SGP.SharedTests.Fixtures;
using SGP.SharedTests.GraphQL;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace SGP.IntegrationTests.GraphQL
{
    [IntegrationTest]
    [Category(TestCategories.GraphQL)]
    public class EstadoSchemaTests : IClassFixture<WebTestApplicationFactory>
    {
        private readonly HttpClient _client;

        public EstadoSchemaTests(WebTestApplicationFactory factory)
        {
            _client = factory.Server.CreateClient();
        }

        [Fact]
        public async Task Devera_RetornarResultadoSucessoComEstados_QuandoObterTodos()
        {
            // Arrange
            const string queryName = QueryNames.ListarEstados;
            var request = new GraphQLQuery<EstadoResponse>(queryName)
                 .AddField(e => e.Regiao)
                 .AddField(e => e.Uf)
                 .AddField(e => e.Nome)
                 .ToGraphQLRequest();

            // Act
            var response = await _client.SendAsync(GraphQLApiEndpoints.Estados, request);

            // Assert
            response.EnsureSuccessStatusCode();

            var data = await response.Content.GetGraphQLDataAsync<IEnumerable<EstadoResponse>>(queryName);
            data.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.HaveCount(Totais.Estados)
                .And.Subject.ForEach(estado =>
                {
                    estado.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2);
                    estado.Regiao.Should().NotBeNullOrWhiteSpace();
                    estado.Nome.Should().NotBeNullOrEmpty();
                });
        }
    }
}
