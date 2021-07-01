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
    public class CidadeRepositoryTests : IClassFixture<EfSqliteFixture>
    {
        private readonly EfSqliteFixture _fixture;

        public CidadeRepositoryTests(EfSqliteFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [ClassData(typeof(TestDatas.FiltrarPorIbge))]
        public async Task Devera_RetornarCidade_QuandoObterPorIbge(int ibge, string cidadeEsperada, string ufEsperada,
            string regiaoEsperada)
        {
            // Arrange
            await _fixture.SeedDataAsync();
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
        [ClassData(typeof(TestDatas.FiltrarPorUf))]
        public async Task Devera_RetornarCidades_QuandoObterPorUf(string uf, int totalEsperado)
        {
            // Arrange
            await _fixture.SeedDataAsync();
            var repository = CriarRepositorio();

            // Act
            var actual = await repository.ObterTodosPorUfAsync(uf);

            // Assert
            actual.Should().NotBeEmpty()
                .And.OnlyHaveUniqueItems()
                .And.HaveCount(totalEsperado)
                .And.Subject.ForEach(cidade =>
                {
                    cidade.Should().NotBeNull();
                    cidade.Id.Should().NotBeEmpty();
                    cidade.EstadoId.Should().NotBeEmpty();
                    cidade.Nome.Should().NotBeNullOrWhiteSpace();
                    cidade.Ibge.Should().BePositive();
                    cidade.Estado.Should().NotBeNull();
                    cidade.Estado.Id.Should().NotBeEmpty();
                    cidade.Estado.RegiaoId.Should().NotBeEmpty();
                    cidade.Estado.Nome.Should().NotBeNullOrWhiteSpace();
                    cidade.Estado.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2).And.Be(uf);
                    cidade.Estado.Regiao.Should().NotBeNull();
                    cidade.Estado.Regiao.Id.Should().NotBeEmpty();
                    cidade.Estado.Regiao.Nome.Should().NotBeNullOrWhiteSpace();
                });
        }

        private ICidadeRepository CriarRepositorio()
            => new CidadeRepository(_fixture.Context);
    }
}