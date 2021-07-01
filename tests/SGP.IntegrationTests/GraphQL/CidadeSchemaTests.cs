using FluentAssertions;
using SGP.Application.Responses;
using SGP.PublicApi.GraphQL.Constants;
using SGP.SharedTests;
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
    public class CidadeSchemaTests : IClassFixture<WebTestApplicationFactory>
    {
        private readonly HttpClient _client;

        public CidadeSchemaTests(WebTestApplicationFactory factory)
        {
            _client = factory.Server.CreateClient();
        }

        [Theory]
        [ClassData(typeof(TestDatas.FiltrarPorUf))]
        public async Task Devera_RetornarCidades_QuandoObterPorUf(string uf, int totalEsperado)
        {
            // Arrange
            const string queryName = QueryNames.CidadesPorEstado;
            var request = new GraphQLQuery<CidadeResponse>(queryName)
                .AddArguments(new { uf })
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

        [Theory]
        [ClassData(typeof(TestDatas.FiltrarPorIbge))]
        public async Task Devera_RetornarResultadoSucessoComCidade_QuandoObterPorIbge(int ibge, string cidadeEsperada,
            string ufEsperada, string regiaoEsperada)
        {
            // Arrange
            const string queryName = QueryNames.CidadePorIbge;
            var request = new GraphQLQuery<CidadeResponse>(queryName)
                .AddArguments(new { ibge })
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
            data.Regiao.Should().NotBeNullOrWhiteSpace().And.Be(regiaoEsperada);
            data.Estado.Should().NotBeNullOrWhiteSpace();
            data.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2).And.Be(ufEsperada);
            data.Nome.Should().NotBeNullOrWhiteSpace().And.Be(cidadeEsperada);
            data.Ibge.Should().BePositive().And.Be(ibge);
        }
    }
}