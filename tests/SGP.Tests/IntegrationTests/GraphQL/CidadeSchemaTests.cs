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
    public class CidadeSchemaTests : IClassFixture<WebTestApplicationFactory>
    {
        private readonly HttpClient _client;

        public CidadeSchemaTests(WebTestApplicationFactory factory)
        {
            _client = factory.Server.CreateClient();
        }

        [Fact]
        public async Task Devera_RetornarCidades_AoObterPorUf()
        {
            // Arrange
            const string queryName = QueryNames.CidadesPorEstado;
            const string ufSaoPaulo = "SP";
            const int totaisEsperado = 645;
            var request = new GraphQLQuery<CidadeResponse>(queryName)
                .AddArguments(new { uf = ufSaoPaulo })
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
            var request = new GraphQLQuery<CidadeResponse>(QueryNames.CidadesPorEstado)
                .AddArguments(new { uf = ufNaoExistente })
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
        public async Task Devera_RetornarErroValidacao_AoObterPorIbgeInexistente()
        {
            // Arrange
            const int ibgeNaoExistente = 999999999;
            var request = new GraphQLQuery<CidadeResponse>(QueryNames.CidadePorIbge)
                .AddArguments(new { ibge = ibgeNaoExistente })
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
        public async Task Devera_RetornarErroValidacao_AoObterPorIbgeInvalido()
        {
            // Arrange
            const int ibgeInvalido = -1;
            var request = new GraphQLQuery<CidadeResponse>(QueryNames.CidadePorIbge)
                .AddArguments(new { ibge = ibgeInvalido })
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
        public async Task Devera_RetornarErroValidacao_AoObterTodosPorUfInvalido()
        {
            // Arrange
            const string ufInvalido = "XXX.XX_X";
            var request = new GraphQLQuery<CidadeResponse>(QueryNames.CidadesPorEstado)
                .AddArguments(new { uf = ufInvalido })
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
        public async Task Devera_RetornarResultadoSucessoComCidade_AoObterPorIbge()
        {
            // Arrange
            const string queryName = QueryNames.CidadePorIbge;
            const int ibgeVotuporanga = 3557105;
            var request = new GraphQLQuery<CidadeResponse>(queryName)
                .AddArguments(new { ibge = ibgeVotuporanga })
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