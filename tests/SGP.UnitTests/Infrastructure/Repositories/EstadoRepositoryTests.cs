using FluentAssertions;
using SGP.Domain.Repositories;
using SGP.Infrastructure.Repositories;
using SGP.SharedTests;
using SGP.SharedTests.Constants;
using SGP.SharedTests.Extensions;
using SGP.SharedTests.Fixtures;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace SGP.UnitTests.Infrastructure.Repositories
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
                .And.Subject.ForEach(estado =>
                {
                    estado.Id.Should().NotBeEmpty();
                    estado.RegiaoId.Should().NotBeEmpty();
                    estado.Nome.Should().NotBeNullOrEmpty();
                    estado.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2);
                    estado.Regiao.Should().NotBeNull();
                    estado.Regiao.Id.Should().NotBeEmpty();
                    estado.Regiao.Nome.Should().NotBeNullOrWhiteSpace().And.Be(regiao);
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
                .And.Subject.ForEach(estado =>
                {
                    estado.Id.Should().NotBeEmpty();
                    estado.RegiaoId.Should().NotBeEmpty();
                    estado.Nome.Should().NotBeNullOrEmpty();
                    estado.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2);
                    estado.Regiao.Should().NotBeNull();
                    estado.Regiao.Id.Should().NotBeEmpty();
                    estado.Regiao.Nome.Should().NotBeNullOrWhiteSpace();
                });
        }

        [Fact]
        public async Task Devera_RetornarListVazia_QuandoObterPorRegiaoInexistente()
        {
            // Arrange
            await _fixture.SeedDataAsync();
            var repository = CriarRepositorio();

            // Act
            var actual = await repository.ObterTodosPorRegiaoAsync("XXX");

            // Assert
            actual.Should().BeEmpty().And.HaveCount(0);
        }

        private IEstadoRepository CriarRepositorio()
            => new EstadoRepository(_fixture.Context);
    }
}