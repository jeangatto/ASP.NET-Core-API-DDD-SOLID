using System.Threading.Tasks;
using FluentAssertions;
using SGP.Domain.Repositories;
using SGP.Infrastructure.Data.Repositories;
using SGP.Tests.Extensions;
using SGP.Tests.Fixtures;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Infrastructure.Data.Repositories;

[UnitTest]
public class CidadeRepositoryTests : IClassFixture<EfSqliteFixture>
{
    private readonly EfSqliteFixture _fixture;

    public CidadeRepositoryTests(EfSqliteFixture fixture) => _fixture = fixture;

    [Theory]
    [InlineData(3550308, "São Paulo", "SP", "Sudeste")]
    [InlineData(3506003, "Bauru", "SP", "Sudeste")]
    [InlineData(3557105, "Votuporanga", "SP", "Sudeste")]
    [InlineData(3304557, "Rio de Janeiro", "RJ", "Sudeste")]
    [InlineData(4205407, "Florianópolis", "SC", "Sul")]
    public async Task Devera_RetornarCidade_AoObterPorIbge(
        int ibge,
        string cidadeEsperada,
        string ufEsperada,
        string regiaoEsperada)
    {
        // Arrange
        var repository = CriarRepositorio();

        // Act
        var actual = await repository.ObterPorIbgeAsync(ibge);

        // Assert
        actual.Should().NotBeNull();
        actual.Id.Should().NotBeEmpty();
        actual.EstadoId.Should().NotBeEmpty();
        actual.Nome.Should().NotBeNullOrWhiteSpace().And.Be(cidadeEsperada);
        actual.Ibge.Should().BePositive().And.Be(ibge);
        actual.Estado.Should().NotBeNull();
        actual.Estado.Id.Should().NotBeEmpty();
        actual.Estado.RegiaoId.Should().NotBeEmpty();
        actual.Estado.Nome.Should().NotBeNullOrWhiteSpace();
        actual.Estado.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2).And.Be(ufEsperada);
        actual.Estado.Regiao.Should().NotBeNull();
        actual.Estado.Regiao.Id.Should().NotBeEmpty();
        actual.Estado.Regiao.Nome.Should().NotBeNullOrWhiteSpace().And.Be(regiaoEsperada);
    }

    [Theory]
    [InlineData("AC", 22)]  // Acre
    [InlineData("AL", 102)] // Alagoas
    [InlineData("AM", 62)]  // Amazonas
    [InlineData("AP", 16)]  // Amapá
    [InlineData("BA", 417)] // Bahia
    [InlineData("CE", 184)] // Ceará
    [InlineData("ES", 78)]  // Espírito Santo
    [InlineData("GO", 246)] // Goiás
    [InlineData("MA", 217)] // Maranhão
    [InlineData("MG", 853)] // Minas Gerais
    [InlineData("MS", 79)]  // Mato Grosso do Sul
    [InlineData("MT", 141)] // Mato Grosso
    [InlineData("PA", 144)] // Pará
    [InlineData("PB", 223)] // Paraíba
    [InlineData("PE", 185)] // Pernambuco
    [InlineData("PI", 224)] // Piauí
    [InlineData("PR", 399)] // Paraná
    [InlineData("RJ", 92)]  // Rio de Janeiro
    [InlineData("RN", 167)] // Rio Grande do Norte
    [InlineData("RO", 52)]  // Rondônia
    [InlineData("RR", 15)]  // Roraima
    [InlineData("RS", 497)] // Rio Grande do Sul
    [InlineData("SC", 295)] // Santa Catarina
    [InlineData("SE", 75)]  // Sergipe
    [InlineData("SP", 645)] // São Paulo
    [InlineData("TO", 139)] // Tocantins
    public async Task Devera_RetornarCidades_AoObterPorUf(string uf, int totalCidadesEsperada)
    {
        // Arrange
        var repository = CriarRepositorio();

        // Act
        var actual = await repository.ObterTodosPorUfAsync(uf);

        // Assert
        actual.Should().NotBeEmpty()
            .And.OnlyHaveUniqueItems()
            .And.HaveCount(totalCidadesEsperada)
            .And.Subject.ForEach(c =>
            {
                c.Id.Should().NotBeEmpty();
                c.EstadoId.Should().NotBeEmpty();
                c.Nome.Should().NotBeNullOrWhiteSpace();
                c.Ibge.Should().BePositive();
                c.Estado.Should().NotBeNull();
                c.Estado.Id.Should().NotBeEmpty();
                c.Estado.RegiaoId.Should().NotBeEmpty();
                c.Estado.Nome.Should().NotBeNullOrWhiteSpace();
                c.Estado.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2).And.Be(uf);
                c.Estado.Regiao.Should().NotBeNull();
                c.Estado.Regiao.Id.Should().NotBeEmpty();
                c.Estado.Regiao.Nome.Should().NotBeNullOrWhiteSpace();
            });
    }

    private ICidadeRepository CriarRepositorio() => new CidadeRepository(_fixture.Context);
}