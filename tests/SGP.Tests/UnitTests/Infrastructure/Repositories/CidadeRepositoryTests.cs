using System.Threading.Tasks;
using FluentAssertions;
using SGP.Domain.Repositories;
using SGP.Infrastructure.Repositories;
using SGP.Tests.Constants;
using SGP.Tests.Extensions;
using SGP.Tests.Fixtures;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Infrastructure.Repositories
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
        public async Task Devera_RetornarCidade_AoObterPorIbge(int ibge, string cidadeEsperada, string ufEsperada,
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
        public async Task Devera_RetornarCidades_AoObterPorUf(string uf, int totalEsperado)
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
                .And.Subject.ForEach(c =>
                {
                    c.Should().NotBeNull();
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
}