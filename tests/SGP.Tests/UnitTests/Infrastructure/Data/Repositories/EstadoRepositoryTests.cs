using System.Threading.Tasks;
using FluentAssertions;
using SGP.Infrastructure.Data.Repositories;
using SGP.Tests.Constants;
using SGP.Tests.Extensions;
using SGP.Tests.Fixtures;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Infrastructure.Data.Repositories;

[UnitTest]
public class EstadoRepositoryTests(EfSqliteFixture fixture) : IClassFixture<EfSqliteFixture>
{
    [Theory]
    [InlineData("Nordeste", 9)]
    [InlineData("Sudeste", 4)]
    [InlineData("Sul", 3)]
    [InlineData("Centro-Oeste", 4)]
    [InlineData("Norte", 7)]
    public async Task Devera_RetornarEstados_AoObterPorRegiao(string regiao, int totalEsperado)
    {
        // Arrange
        var repository = CriarRepositorio();

        // Act
        var actual = await repository.ObterTodosPorRegiaoAsync(regiao);

        // Assert
        actual.Should().NotBeEmpty()
            .And.OnlyHaveUniqueItems()
            .And.HaveCount(totalEsperado)
            .And.Subject.ForEach(e =>
            {
                e.Id.Should().NotBeEmpty();
                e.RegiaoId.Should().NotBeEmpty();
                e.Nome.Should().NotBeNullOrWhiteSpace();
                e.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2);
                e.Regiao.Should().NotBeNull();
                e.Regiao.Id.Should().NotBeEmpty();
                e.Regiao.Nome.Should().NotBeNullOrWhiteSpace().And.Be(regiao);
            });
    }

    [Fact]
    public async Task Devera_RetornarEstados_AoObterTodos()
    {
        // Arrange
        var repository = CriarRepositorio();

        // Act
        var actual = await repository.ObterTodosAsync();

        // Assert
        actual.Should().NotBeEmpty()
            .And.OnlyHaveUniqueItems()
            .And.HaveCount(Totais.Estados)
            .And.Subject.ForEach(e =>
            {
                e.Id.Should().NotBeEmpty();
                e.RegiaoId.Should().NotBeEmpty();
                e.Nome.Should().NotBeNullOrWhiteSpace();
                e.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2);
                e.Regiao.Should().NotBeNull();
                e.Regiao.Id.Should().NotBeEmpty();
                e.Regiao.Nome.Should().NotBeNullOrWhiteSpace();
            });
    }

    private EstadoRepository CriarRepositorio() => new(fixture.Context);
}