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
    [UnitTest(TestCategories.Infrastructure)]
    public class CidadeRepositoryTests : IClassFixture<EfSqliteFixture>
    {
        private readonly EfSqliteFixture _fixture;

        public CidadeRepositoryTests(EfSqliteFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Devera_RetornarCidade_QuandoObterPorIbge()
        {
            // Arrange
            await _fixture.PopularBaseDadosAsync();
            var repository = CriarRepositorio();
            const int ibge = 3557105;

            // Act
            var actual = await repository.ObterPorIbgeAsync(ibge);

            // Assert
            actual.Should().NotBeNull();
            actual.Id.Should().NotBeEmpty();
            actual.EstadoId.Should().NotBeEmpty();
            actual.Nome.Should().NotBeNullOrWhiteSpace().And.Be("Votuporanga");
            actual.Ibge.Should().BePositive().And.Be(ibge);
            actual.Estado.Should().NotBeNull();
            actual.Estado.Id.Should().NotBeEmpty();
            actual.Estado.RegiaoId.Should().NotBeEmpty();
            actual.Estado.Nome.Should().NotBeNullOrWhiteSpace().And.Be("São Paulo");
            actual.Estado.Uf.Should().NotBeNullOrWhiteSpace().And.HaveLength(2).And.Be("SP");
            actual.Estado.Regiao.Should().NotBeNull();
            actual.Estado.Regiao.Id.Should().NotBeEmpty();
            actual.Estado.Regiao.Nome.Should().NotBeNullOrWhiteSpace().And.Be("Sudeste");
        }

        [Theory]
        [ClassData(typeof(EstadosTestData))]
        public async Task Devera_RetornarCidades_QuandoObterPorUf(string uf, int totalEsperado)
        {
            // Arrange
            await _fixture.PopularBaseDadosAsync();
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

        [Fact]
        public async Task Devera_RetornarListVazia_QuandoObterPorUfInexistente()
        {
            // Arrange
            await _fixture.PopularBaseDadosAsync();
            var repository = CriarRepositorio();
            const string uf = "TX";

            // Act
            var actual = await repository.ObterTodosPorUfAsync(uf);

            // Assert
            actual.Should().BeEmpty().And.HaveCount(0);
        }

        [Fact]
        public async Task Devera_RetornarNulo_QuandoObterPorIbgeInexistente()
        {
            // Arrange
            await _fixture.PopularBaseDadosAsync();
            var repository = CriarRepositorio();
            const int ibge = 999999999;

            // Act
            var actual = await repository.ObterPorIbgeAsync(ibge);

            // Assert
            actual.Should().BeNull();
        }

        private ICidadeRepository CriarRepositorio() => new CidadeRepository(_fixture.Context);

        private class EstadosTestData : TheoryData<string, int>
        {
            public EstadosTestData()
            {
                Add("AC", 22);  // Acre
                Add("AL", 102); // Alagoas
                Add("AM", 62);  // Amazonas
                Add("AP", 16);  // Amapá
                Add("BA", 417); // Bahia
                Add("CE", 184); // Ceará
                Add("ES", 78);  // Espírito Santo
                Add("GO", 246); // Goiás
                Add("MA", 217); // Maranhão
                Add("MG", 853); // Minas Gerais
                Add("MS", 79);  // Mato Grosso do Sul
                Add("MT", 141); // Mato Grosso
                Add("PA", 144); // Pará
                Add("PB", 223); // Paraíba
                Add("PE", 185); // Pernambuco
                Add("PI", 224); // Piauí
                Add("PR", 399); // Paraná
                Add("RJ", 92);  // Rio de Janeiro
                Add("RN", 167); // Rio Grande do Norte
                Add("RO", 52);  // Rondônia
                Add("RR", 15);  // Roraima
                Add("RS", 497); // Rio Grande do Sul
                Add("SC", 295); // Santa Catarina
                Add("SE", 75);  // Sergipe
                Add("SP", 645); // São Paulo
                Add("TO", 139); // Tocantins
            }
        }
    }
}