using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using SGP.Application.Responses;
using SGP.PublicApi.GraphQL.Constants;
using SGP.Tests.Constants;
using SGP.Tests.Extensions;
using SGP.Tests.Fixtures;
using SGP.Tests.Models;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.IntegrationTests.GraphQL
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
        public async Task Devera_RetornarResultadoSucessoComEstados_AoObterTodos()
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
                .And.Subject.ForEach(e =>
                {
                    e.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2);
                    e.Regiao.Should().NotBeNullOrWhiteSpace();
                    e.Nome.Should().NotBeNullOrEmpty();
                });
        }
    }
}
