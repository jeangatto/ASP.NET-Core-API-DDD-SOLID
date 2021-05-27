using FluentAssertions;
using SGP.Domain.Repositories;
using SGP.Infrastructure.Repositories;
using SGP.Tests.Extensions;
using SGP.Tests.Fixtures;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Infrastructure.Repositories
{
    [Category(TestCategories.Infrastructure)]
    public class RegiaoRepositorioTests : UnitTestBase, IClassFixture<EfSqliteFixture>
    {
        private readonly EfSqliteFixture _fixture;

        public RegiaoRepositorioTests(EfSqliteFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Devera_ObterTodasRegioes_RetornaRegioes()
        {
            // Arrange
            await _fixture.SeedAsync();
            var repository = CriarRepositorio();

            // Act
            var actual = await repository.ObterTodosAsync();

            // Assert
            actual.Should().NotBeEmpty()
                .And.OnlyHaveUniqueItems()
                .And.HaveCount(5)
                .And.Subject.ForEach(regiao =>
                {
                    regiao.Id.Should().NotBeEmpty();
                    regiao.Nome.Should().NotBeNullOrWhiteSpace();
                });
        }

        private IRegiaoRepositorio CriarRepositorio()
            => new RegiaoRepositorio(_fixture.Context);
    }
}