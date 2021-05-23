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
    public class EstadoRepositoryTests : UnitTestBase, IClassFixture<EfSqliteFixture>
    {
        private readonly EfSqliteFixture _fixture;

        public EstadoRepositoryTests(EfSqliteFixture fixture) => _fixture = fixture;

        [Theory]
        [ClassData(typeof(EstadoTestData))]
        public async Task Devera_RetornarEstados_QuandoObterPorRegiao(string regiao, int totalEsperado)
        {
            // Arrange
            await _fixture.PopularAsync();
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
            await _fixture.PopularAsync();
            var repository = CriarRepositorio();

            // Act
            var actual = await repository.ObterTodosAsync();

            // Assert
            actual.Should().NotBeEmpty()
                .And.OnlyHaveUniqueItems()
                .And.HaveCount(27)
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
            await _fixture.PopularAsync();
            var repository = CriarRepositorio();
            const string regiao = "NaoExistente";

            // Act
            var actual = await repository.ObterTodosPorRegiaoAsync(regiao);

            // Assert
            actual.Should().BeEmpty().And.HaveCount(0);
        }

        private IEstadoRepository CriarRepositorio()
            => new EstadoRepository(_fixture.Context);

        /// <summary>
        /// REF: https://pt.wikipedia.org/wiki/Lista_de_estados_brasileiros_por_n%C3%BAmero_de_munic%C3%ADpios
        /// </summary>
        private class EstadoTestData : TheoryData<string, int>
        {
            public EstadoTestData()
            {
                Add("Nordeste", 9);
                Add("Sudeste", 4);
                Add("Sul", 3);
                Add("Centro-Oeste", 4);
                Add("Norte", 7);
            }
        }
    }
}