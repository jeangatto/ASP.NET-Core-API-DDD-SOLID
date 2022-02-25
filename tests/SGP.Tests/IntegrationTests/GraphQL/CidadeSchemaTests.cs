using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using SGP.Application.Responses;
using SGP.PublicApi.GraphQL.Constants;
using SGP.Tests.Extensions;
using SGP.Tests.Fixtures;
using SGP.Tests.Models;
using Xunit;
using Xunit.Abstractions;

namespace SGP.Tests.IntegrationTests.GraphQL
{
    public class CidadeSchemaTests : IntegrationTestBase, IClassFixture<WebTestApplicationFactory>
    {
        public CidadeSchemaTests(WebTestApplicationFactory factory, ITestOutputHelper output) : base(factory, output)
        {
        }

        [Fact]
        public async Task Devera_RetornarResultadoSucessoComCidades_AoObterPorUf()
        {
            // Arrange
            const int total = 645;
            const string uf = "SP";
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
            var response = await HttpClient.SendAsync(Output, EndPoints.Api.Cidades, request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var data = await response.Content.GetGraphDataAsync<IEnumerable<CidadeResponse>>(queryName);
            data.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.HaveCount(total)
                .And.Subject.ForEach(c =>
                {
                    c.Regiao.Should().NotBeNullOrWhiteSpace();
                    c.Estado.Should().NotBeNullOrWhiteSpace();
                    c.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2).And.Be(uf);
                    c.Nome.Should().NotBeNullOrWhiteSpace();
                    c.Ibge.Should().BePositive();
                });
        }

        [Fact]
        public async Task Devera_RetornarErroNaoEncontrado_AoObterTodosPorUfInexistente()
        {
            // Arrange
            var request = new GraphQLQuery<CidadeResponse>(QueryNames.CidadesPorEstado)
                .AddArguments(new { uf = "XX" })
                .AddField(c => c.Nome)
                .ToGraphQLRequest();

            // Act
            var response = await HttpClient.SendAsync(Output, EndPoints.Api.Cidades, request);

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
            var request = new GraphQLQuery<CidadeResponse>(QueryNames.CidadePorIbge)
                .AddArguments(new { ibge = int.MaxValue })
                .AddField(c => c.Ibge)
                .ToGraphQLRequest();

            // Act
            var response = await HttpClient.SendAsync(Output, EndPoints.Api.Cidades, request);

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
            var request = new GraphQLQuery<CidadeResponse>(QueryNames.CidadePorIbge)
                .AddArguments(new { ibge = int.MinValue })
                .AddField(c => c.Ibge)
                .ToGraphQLRequest();

            // Act
            var response = await HttpClient.SendAsync(Output, EndPoints.Api.Cidades, request);

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
            var request = new GraphQLQuery<CidadeResponse>(QueryNames.CidadesPorEstado)
                .AddArguments(new { uf = "XXX.XX_X" })
                .AddField(c => c.Nome)
                .ToGraphQLRequest();

            // Act
            var response = await HttpClient.SendAsync(Output, EndPoints.Api.Cidades, request);

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
            const int ibge = 3557105;
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
            var response = await HttpClient.SendAsync(Output, EndPoints.Api.Cidades, request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var data = await response.Content.GetGraphDataAsync<CidadeResponse>(queryName);
            data.Should().NotBeNull();
            data.Regiao.Should().NotBeNullOrWhiteSpace();
            data.Estado.Should().NotBeNullOrWhiteSpace();
            data.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2);
            data.Nome.Should().NotBeNullOrWhiteSpace();
            data.Ibge.Should().BePositive().And.Be(ibge);
        }
    }
}