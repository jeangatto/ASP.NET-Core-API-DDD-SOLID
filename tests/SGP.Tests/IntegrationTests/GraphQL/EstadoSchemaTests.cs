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
using Xunit.Abstractions;

namespace SGP.Tests.IntegrationTests.GraphQL
{
    public class EstadoSchemaTests : IntegrationTestBase, IClassFixture<WebTestApplicationFactory>
    {
        public EstadoSchemaTests(WebTestApplicationFactory factory, ITestOutputHelper outputHelper) : base(factory, outputHelper)
        {
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
            var response = await HttpClient.SendAsync(OutputHelper, EndPoints.Api.Estados, request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var data = await response.Content.GetGraphDataAsync<IEnumerable<EstadoResponse>>(queryName);
            data.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.HaveCount(Totais.Estados)
                .And.Subject.ForEach(e =>
                {
                    e.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2);
                    e.Regiao.Should().NotBeNullOrWhiteSpace();
                    e.Nome.Should().NotBeNullOrWhiteSpace();
                });
        }
    }
}