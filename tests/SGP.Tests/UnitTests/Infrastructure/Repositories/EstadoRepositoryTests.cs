using FluentAssertions;
using SGP.Domain.Repositories;
using SGP.Infrastructure.Repositories;
using SGP.Tests.Constants;
using SGP.Tests.Extensions;
using SGP.Tests.Fixtures;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Infrastructure.Repositories
{
    [UnitTest(TestCategories.Infrastructure)]
    public class EstadoRepositoryTests : IClassFixture<EfSqliteFixture>
    {
        private readonly EfSqliteFixture _fixture;

        public EstadoRepositoryTests(EfSqliteFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [ClassData(typeof(TestDatas.FiltrarEstadoPorRegiao))]
        public async Task Devera_RetornarEstados_QuandoObterPorRegiao(string regiao, int totalEsperado)
        {
            // Arrange
            await _fixture.SeedDataAsync();
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
                    e.Nome.Should().NotBeNullOrEmpty();
                    e.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2);
                    e.Regiao.Should().NotBeNull();
                    e.Regiao.Id.Should().NotBeEmpty();
                    e.Regiao.Nome.Should().NotBeNullOrWhiteSpace().And.Be(regiao);
                });
        }

        [Fact]
        public async Task Devera_RetornarEstados_QuandoObterTodos()
        {
            // Arrange
            await _fixture.SeedDataAsync();
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
                    e.Nome.Should().NotBeNullOrEmpty();
                    e.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2);
                    e.Regiao.Should().NotBeNull();
                    e.Regiao.Id.Should().NotBeEmpty();
                    e.Regiao.Nome.Should().NotBeNullOrWhiteSpace();
                });
        }

        private IEstadoRepository CriarRepositorio() => new EstadoRepository(_fixture.Context);
    }
}