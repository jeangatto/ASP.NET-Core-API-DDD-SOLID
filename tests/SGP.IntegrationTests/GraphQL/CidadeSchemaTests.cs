using FluentAssertions;
using SGP.Application.Responses;
using SGP.IntegrationTests.Extensions;
using SGP.IntegrationTests.Fixtures;
using SGP.IntegrationTests.Models;
using SGP.PublicApi.GraphQL.Constants;
using SGP.SharedTests.Constants;
using SGP.SharedTests.Extensions;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace SGP.IntegrationTests.GraphQL
{
    [IntegrationTest]
    [Category(TestCategories.GraphQL)]
    public class CidadeSchemaTests : IClassFixture<WebTestApplicationFactory>
    {
        private readonly HttpClient _client;

        public CidadeSchemaTests(WebTestApplicationFactory factory)
        {
            _client = factory.Server.CreateClient();
        }

        [Fact]
        public async Task Devera_RetornarCidades_QuandoObterPorUf()
        {
            // Arrange
            const string queryName = QueryNames.CidadesPorEstado;
            const int totaisEsperado = 645;
            var request = new GraphQLQuery<CidadeResponse>(queryName)
                .AddArguments(new { uf = "SP" })
                .AddField(c => c.Regiao)
                .AddField(c => c.Estado)
                .AddField(c => c.Uf)
                .AddField(c => c.Nome)
                .AddField(c => c.Ibge)
                .ToGraphQLRequest();

            // Act
            var response = await _client.SendAsync(GraphQLApiEndpoints.Cidades, request);

            // Assert
            response.EnsureSuccessStatusCode();

            var data = await response.Content.GetGraphQLDataAsync<IEnumerable<CidadeResponse>>(queryName);
            data.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.HaveCount(totaisEsperado)
                .And.Subject.ForEach(cidade =>
                {
                    cidade.Regiao.Should().NotBeNullOrWhiteSpace();
                    cidade.Estado.Should().NotBeNullOrWhiteSpace();
                    cidade.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2);
                    cidade.Nome.Should().NotBeNullOrWhiteSpace();
                    cidade.Ibge.Should().BePositive();
                });
        }

        [Fact]
        public async Task Devera_RetornarErroNaoEncontrado_QuandoObterTodosPorUfInexistente()
        {
            // Arrange
            var request = new GraphQLQuery<CidadeResponse>(QueryNames.CidadesPorEstado)
                .AddArguments(new { uf = "XX" })
                .AddField(c => c.Nome)
                .ToGraphQLRequest();

            // Act
            var response = await _client.SendAsync(GraphQLApiEndpoints.Cidades, request);

            // Assert
            response.EnsureSuccessStatusCode();
            var errors = await response.Content.GetGraphQLErrors();
            errors.Should().NotBeNullOrEmpty().And.OnlyHaveUniqueItems();
        }

        [Fact]
        public async Task Devera_RetornarErroValidacao_QuandoObterPorIbgeInexistente()
        {
            // Arrange
            var request = new GraphQLQuery<CidadeResponse>(QueryNames.CidadePorIbge)
                .AddArguments(new { ibge = int.MaxValue })
                .AddField(c => c.Ibge)
                .ToGraphQLRequest();

            // Act
            var response = await _client.SendAsync(GraphQLApiEndpoints.Cidades, request);

            // Assert
            response.EnsureSuccessStatusCode();
            var errors = await response.Content.GetGraphQLErrors();
            errors.Should().NotBeNullOrEmpty().And.OnlyHaveUniqueItems();
        }

        [Fact]
        public async Task Devera_RetornarErroValidacao_QuandoObterPorIbgeInvalido()
        {
            // Arrange
            var request = new GraphQLQuery<CidadeResponse>(QueryNames.CidadePorIbge)
                .AddArguments(new { ibge = -1 })
                .AddField(c => c.Ibge)
                .ToGraphQLRequest();

            // Act
            var response = await _client.SendAsync(GraphQLApiEndpoints.Cidades, request);

            // Assert
            response.EnsureSuccessStatusCode();
            var errors = await response.Content.GetGraphQLErrors();
            errors.Should().NotBeNullOrEmpty().And.OnlyHaveUniqueItems();
        }

        [Fact]
        public async Task Devera_RetornarErroValidacao_QuandoObterTodosPorUfInvalido()
        {
            // Arrange
            var request = new GraphQLQuery<CidadeResponse>(QueryNames.CidadesPorEstado)
                .AddArguments(new { uf = "XXX XXX" })
                .AddField(c => c.Nome)
                .ToGraphQLRequest();

            // Act
            var response = await _client.SendAsync(GraphQLApiEndpoints.Cidades, request);

            // Assert
            response.EnsureSuccessStatusCode();
            var errors = await response.Content.GetGraphQLErrors();
            errors.Should().NotBeNullOrEmpty().And.OnlyHaveUniqueItems();
        }

        [Fact]
        public async Task Devera_RetornarResultadoSucessoComCidade_QuandoObterPorIbge()
        {
            // Arrange
            const string queryName = QueryNames.CidadePorIbge;
            var request = new GraphQLQuery<CidadeResponse>(queryName)
                .AddArguments(new { ibge = 3557105 })
                .AddField(c => c.Regiao)
                .AddField(c => c.Estado)
                .AddField(c => c.Uf)
                .AddField(c => c.Nome)
                .AddField(c => c.Ibge)
                .ToGraphQLRequest();

            // Act
            var response = await _client.SendAsync(GraphQLApiEndpoints.Cidades, request);

            // Assert
            response.EnsureSuccessStatusCode();

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