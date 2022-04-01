using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using SGP.Application.Interfaces;
using SGP.Application.Mapper;
using SGP.Application.Requests.CidadeRequests;
using SGP.Application.Services;
using SGP.Infrastructure.Repositories;
using SGP.Shared.Errors;
using SGP.Tests.Extensions;
using SGP.Tests.Fixtures;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Application.Services;

[UnitTest]
public class CidadeServiceTests : IClassFixture<EfSqliteFixture>
{
    private readonly EfSqliteFixture _fixture;

    public CidadeServiceTests(EfSqliteFixture fixture) => _fixture = fixture;

    [Fact]
    public async Task Devera_RetornarErroValidacao_AoObterTodosPorUfInvalido()
    {
        // Arrange
        var request = new ObterTodosPorUfRequest(string.Empty);
        var service = CriarServico();

        // Act
        var actual = await service.ObterTodosPorUfAsync(request);

        // Assert
        actual.Should().NotBeNull();
        actual.HasError<ValidationError>().Should().BeTrue();
        actual.IsFailed.Should().BeTrue();
        actual.Errors.Should().NotBeNullOrEmpty()
            .And.OnlyHaveUniqueItems()
            .And.Subject.ForEach(error => error.Message.Should().NotBeNullOrWhiteSpace());
    }

    [Fact]
    public async Task Devera_RetornarErroNaoEncontrado_AoObterTodosPorUfInexistente()
    {
        // Arrange
        var request = new ObterTodosPorUfRequest("XX");
        var expectedError = $"Nenhuma cidade encontrada pelo UF: {request.Uf}";
        var service = CriarServico();

        // Act
        var actual = await service.ObterTodosPorUfAsync(request);

        // Assert
        actual.Should().NotBeNull();
        actual.HasError<NotFoundError>().Should().BeTrue();
        actual.IsFailed.Should().BeTrue();
        actual.Errors.Should().NotBeNullOrEmpty()
            .And.OnlyHaveUniqueItems()
            .And.SatisfyRespectively(error => error.Message.Should().NotBeNullOrWhiteSpace().And.Be(expectedError));
    }

    [Fact]
    public async Task Devera_RetornarResultadoSucessoComCidades_AoObterTodosPorUf()
    {
        // Arrange
        const int totalCidades = 645;
        const string ufSaoPaulo = "SP";
        var request = new ObterTodosPorUfRequest(ufSaoPaulo);
        var service = CriarServico();

        // Act
        var actual = await service.ObterTodosPorUfAsync(request);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeTrue();
        actual.Value.Should().NotBeNullOrEmpty()
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
    public async Task Devera_RetornarResultadoSucessoComCidade_AoObterPorIbge()
    {
        // Arrange
        const int ibgeVotuporanga = 3557105;
        var request = new ObterPorIbgeRequest(ibgeVotuporanga);
        var service = CriarServico();

        // Act
        var actual = await service.ObterPorIbgeAsync(request);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeTrue();
        actual.Value.Should().NotBeNull();
        var cidadeResponse = actual.Value;
        cidadeResponse.Regiao.Should().NotBeNullOrWhiteSpace();
        cidadeResponse.Estado.Should().NotBeNullOrWhiteSpace();
        cidadeResponse.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2);
        cidadeResponse.Nome.Should().NotBeNullOrWhiteSpace();
        cidadeResponse.Ibge.Should().BePositive().And.Be(ibgeVotuporanga);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public async Task Devera_RetornarErroValidacao_AoObterPorIbgeInvalido(int ibge)
    {
        // Arrange
        var request = new ObterPorIbgeRequest(ibge);
        var service = CriarServico();

        // Act
        var actual = await service.ObterPorIbgeAsync(request);

        // Assert
        actual.Should().NotBeNull();
        actual.HasError<ValidationError>().Should().BeTrue();
        actual.IsFailed.Should().BeTrue();
        actual.Errors.Should().NotBeNullOrEmpty()
            .And.OnlyHaveUniqueItems()
            .And.Subject.ForEach(error => error.Message.Should().NotBeNullOrWhiteSpace());
    }

    [Fact]
    public async Task Devera_RetornarErroValidacao_AoObterPorIbgeInexistente()
    {
        // Arrange
        var request = new ObterPorIbgeRequest(int.MaxValue);
        var expectedError = $"Nenhuma cidade encontrada pelo IBGE: {request.Ibge}";
        var service = CriarServico();

        // Act
        var actual = await service.ObterPorIbgeAsync(request);

        // Assert
        actual.Should().NotBeNull();
        actual.HasError<NotFoundError>().Should().BeTrue();
        actual.IsFailed.Should().BeTrue();
        actual.Errors.Should().NotBeNullOrEmpty()
            .And.OnlyHaveUniqueItems()
            .And.SatisfyRespectively(error => error.Message.Should().NotBeNullOrWhiteSpace().And.Be(expectedError));
    }

    private ICidadeService CriarServico()
    {
        var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<DomainToResponseMapper>()));
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var repositorio = new CidadeRepository(_fixture.Context);
        return new CidadeService(mapper, memoryCache, repositorio);
    }
}