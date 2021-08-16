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
    public class CidadeSchemaTests : IntegrationTestBase, IClassFixture<WebTestApplicationFactory>
    {
        public CidadeSchemaTests(WebTestApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Devera_RetornarCidades_AoObterPorUf()
        {
            // Arrange
            const string queryName = QueryNames.CidadesPorEstado;
            const string ufSaoPaulo = "SP";
            const int totalCidadesEsperado = 645;
            var request = new GraphQuery<CidadeResponse>(queryName)
                .AddArguments(new { uf = ufSaoPaulo })
                .AddField(c => c.Regiao)
                .AddField(c => c.Estado)
                .AddField(c => c.Uf)
                .AddField(c => c.Nome)
                .AddField(c => c.Ibge)
                .ToGraphQLRequest();

            // Act
            var response = await Client.SendAsync(GraphQLApiEndpoints.Cidades, request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var data = await response.Content.GetGraphQLDataAsync<IEnumerable<CidadeResponse>>(queryName);
            data.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.HaveCount(totalCidadesEsperado)
                .And.Subject.ForEach(c =>
                {
                    c.Regiao.Should().NotBeNullOrWhiteSpace();
                    c.Estado.Should().NotBeNullOrWhiteSpace();
                    c.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2);
                    c.Nome.Should().NotBeNullOrWhiteSpace();
                    c.Ibge.Should().BePositive();
                });
        }

        [Fact]
        public async Task Devera_RetornarErroNaoEncontrado_AoObterTodosPorUfInexistente()
        {
            // Arrange
            const string ufNaoExistente = "XX";
            var request = new GraphQuery<CidadeResponse>(QueryNames.CidadesPorEstado)
                .AddArguments(new { uf = ufNaoExistente })
                .AddField(c => c.Nome)
                .ToGraphQLRequest();

            // Act
            var response = await Client.SendAsync(GraphQLApiEndpoints.Cidades, request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var errors = await response.Content.GetGraphQLErrors();
            errors.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.Subject.ForEach(error => error.Message.Should().NotBeNullOrEmpty());
        }

        [Fact]
        public async Task Devera_RetornarErroValidacao_AoObterPorIbgeInexistente()
        {
            // Arrange
            const int ibgeNaoExistente = 999999999;
            var request = new GraphQuery<CidadeResponse>(QueryNames.CidadePorIbge)
                .AddArguments(new { ibge = ibgeNaoExistente })
                .AddField(c => c.Ibge)
                .ToGraphQLRequest();

            // Act
            var response = await Client.SendAsync(GraphQLApiEndpoints.Cidades, request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var errors = await response.Content.GetGraphQLErrors();
            errors.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.Subject.ForEach(error => error.Message.Should().NotBeNullOrEmpty());
        }

        [Fact]
        public async Task Devera_RetornarErroValidacao_AoObterPorIbgeInvalido()
        {
            // Arrange
            const int ibgeInvalido = -1;
            var request = new GraphQuery<CidadeResponse>(QueryNames.CidadePorIbge)
                .AddArguments(new { ibge = ibgeInvalido })
                .AddField(c => c.Ibge)
                .ToGraphQLRequest();

            // Act
            var response = await Client.SendAsync(GraphQLApiEndpoints.Cidades, request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var errors = await response.Content.GetGraphQLErrors();
            errors.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.Subject.ForEach(error => error.Message.Should().NotBeNullOrEmpty());
        }

        [Fact]
        public async Task Devera_RetornarErroValidacao_AoObterTodosPorUfInvalido()
        {
            // Arrange
            const string ufInvalido = "XXX.XX_X";
            var request = new GraphQuery<CidadeResponse>(QueryNames.CidadesPorEstado)
                .AddArguments(new { uf = ufInvalido })
                .AddField(c => c.Nome)
                .ToGraphQLRequest();

            // Act
            var response = await Client.SendAsync(GraphQLApiEndpoints.Cidades, request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var errors = await response.Content.GetGraphQLErrors();
            errors.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.Subject.ForEach(error => error.Message.Should().NotBeNullOrEmpty());
        }

        [Fact]
        public async Task Devera_RetornarResultadoSucessoComCidade_AoObterPorIbge()
        {
            // Arrange
            const string queryName = QueryNames.CidadePorIbge;
            const int ibgeVotuporanga = 3557105;
            var request = new GraphQuery<CidadeResponse>(queryName)
                .AddArguments(new { ibge = ibgeVotuporanga })
                .AddField(c => c.Regiao)
                .AddField(c => c.Estado)
                .AddField(c => c.Uf)
                .AddField(c => c.Nome)
                .AddField(c => c.Ibge)
                .ToGraphQLRequest();

            // Act
            var response = await Client.SendAsync(GraphQLApiEndpoints.Cidades, request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var data = await response.Content.GetGraphQLDataAsync<CidadeResponse>(queryName);
            data.Should().NotBeNull();
            data.Regiao.Should().NotBeNullOrWhiteSpace();
            data.Estado.Should().NotBeNullOrWhiteSpace();
            data.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2);
            data.Nome.Should().NotBeNullOrWhiteSpace();
            data.Ibge.Should().BePositive();
        }
    }
}