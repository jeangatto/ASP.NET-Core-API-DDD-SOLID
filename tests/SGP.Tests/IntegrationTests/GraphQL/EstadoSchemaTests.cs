using System.Collections.Generic;
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
    public class EstadoSchemaTests : IntegrationTestBase, IClassFixture<WebTestApplicationFactory>
    {
        public EstadoSchemaTests(WebTestApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Devera_RetornarResultadoSucessoComEstados_AoObterTodos()
        {
            // Arrange
            const string queryName = QueryNames.ListarEstados;
            var request = new GraphQuery<EstadoResponse>(queryName)
                .AddField(e => e.Regiao)
                .AddField(e => e.Uf)
                .AddField(e => e.Nome)
                .ToGraphQLRequest();

            // Act
            var response = await Client.SendAsync(GraphQLApiEndpoints.Estados, request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
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