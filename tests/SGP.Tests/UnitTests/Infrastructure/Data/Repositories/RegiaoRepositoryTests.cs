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
public class RegiaoRepositoryTests(EfSqliteFixture fixture) : IClassFixture<EfSqliteFixture>
{
    [Fact]
    public async Task Devera_ObterTodasRegioes_RetornaRegioes()
    {
        // Arrange
        var repository = CriarRepositorio();

        // Act
        var actual = await repository.ObterTodosAsync();

        // Assert
        actual.Should().NotBeEmpty()
            .And.OnlyHaveUniqueItems()
            .And.HaveCount(Totais.Regioes)
            .And.Subject.ForEach(r =>
            {
                r.Id.Should().NotBeEmpty();
                r.Nome.Should().NotBeNullOrWhiteSpace();
            });
    }

    private RegiaoRepository CriarRepositorio() => new(fixture.Context);
}