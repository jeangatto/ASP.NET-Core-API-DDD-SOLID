using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SGP.Infrastructure.Context;
using SGP.SharedTests;
using SGP.SharedTests.Fixtures;
using SGP.SharedTests.Mocks;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace SGP.UnitTests.Infrastructure.Context
{
    [UnitTest(TestCategories.Infrastructure)]
    public class SgpContextSeedTests : IClassFixture<EfSqliteFixture>
    {
        private readonly EfSqliteFixture _fixture;

        public SgpContextSeedTests(EfSqliteFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Should_ReturnsRowsAffected_WhenEnsureSeedData()
        {
            // Arrange
            var context = _fixture.Context;

            // Act
            var actual = await context.EnsureSeedDataAsync(LoggerFactoryMock.Create());
            var totalRegioes = await context.Regioes.AsNoTracking().LongCountAsync();
            var totalEstados = await context.Estados.AsNoTracking().LongCountAsync();
            var totalCidades = await context.Cidades.AsNoTracking().LongCountAsync();

            // Assert
            actual.Should().Be(totalRegioes + totalEstados + totalCidades);
            totalRegioes.Should().Be(5);
            totalEstados.Should().Be(27);
            totalCidades.Should().Be(5570);
        }
    }
}