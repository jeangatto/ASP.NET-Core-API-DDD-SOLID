using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using SGP.Application.Responses;
using SGP.PublicApi.GraphQL.Constants;
using SGP.Tests.Extensions;
using SGP.Tests.Fixtures;
using SGP.Tests.Models;
using Xunit;

namespace SGP.Tests.IntegrationTests.GraphQL
{
    public class CidadeSchemaTests : IntegrationTestBase, IClassFixture<WebTestApplicationFactory>
    {
        public CidadeSchemaTests(WebTestApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Devera_RetornarCidades_AoObterPorUf()
        {
            // Arrange
            const int totalCidades = 645;
            const string ufSaoPaulo = "SP";
            const string queryName = QueryNames.CidadesPorEstado;

            var request = new GraphQLQuery<CidadeResponse>(queryName)
                .AddArguments(new { uf = ufSaoPaulo })
                .AddField(c => c.Regiao)
                .AddField(c => c.Estado)
                .AddField(c => c.Uf)
                .AddField(c => c.Nome)
                .AddField(c => c.Ibge)
                .ToGraphQLRequest();

            // Act
            var response = await HttpClient.SendAsync(EndPoints.Api.Cidades, request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var data = await response.Content.GetGraphDataAsync<IEnumerable<CidadeResponse>>(queryName);
            data.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.HaveCount(totalCidades)
                .And.Subject.ForEach(c =>
                {
                    c.Regiao.Should().NotBeNullOrWhiteSpace();
                    c.Estado.Should().NotBeNullOrWhiteSpace();
                    c.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2).And.Be(ufSaoPaulo);
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
            var response = await HttpClient.SendAsync(EndPoints.Api.Cidades, request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var errors = await response.Content.GetGraphErrorsAsync();
            errors.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.Subject.ForEach(error => error.Message.Should().NotBeNullOrWhiteSpace());
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
            var response = await HttpClient.SendAsync(EndPoints.Api.Cidades, request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var errors = await response.Content.GetGraphErrorsAsync();
            errors.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.Subject.ForEach(error => error.Message.Should().NotBeNullOrWhiteSpace());
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
            var response = await HttpClient.SendAsync(EndPoints.Api.Cidades, request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var errors = await response.Content.GetGraphErrorsAsync();
            errors.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.Subject.ForEach(error => error.Message.Should().NotBeNullOrWhiteSpace());
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
            var response = await HttpClient.SendAsync(EndPoints.Api.Cidades, request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var errors = await response.Content.GetGraphErrorsAsync();
            errors.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.Subject.ForEach(error => error.Message.Should().NotBeNullOrWhiteSpace());
        }

        [Fact]
        public async Task Devera_RetornarResultadoSucessoComCidade_AoObterPorIbge()
        {
            // Arrange
            const int ibgeVotuporanga = 3557105;
            const string queryName = QueryNames.CidadePorIbge;

            var request = new GraphQLQuery<CidadeResponse>(queryName)
                .AddArguments(new { ibge = ibgeVotuporanga })
                .AddField(c => c.Regiao)
                .AddField(c => c.Estado)
                .AddField(c => c.Uf)
                .AddField(c => c.Nome)
                .AddField(c => c.Ibge)
                .ToGraphQLRequest();

            // Act
            var response = await HttpClient.SendAsync(EndPoints.Api.Cidades, request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var data = await response.Content.GetGraphDataAsync<CidadeResponse>(queryName);
            data.Should().NotBeNull();
            data.Regiao.Should().NotBeNullOrWhiteSpace();
            data.Estado.Should().NotBeNullOrWhiteSpace();
            data.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2);
            data.Nome.Should().NotBeNullOrWhiteSpace();
            data.Ibge.Should().BePositive().And.Be(ibgeVotuporanga);
        }
    }
}