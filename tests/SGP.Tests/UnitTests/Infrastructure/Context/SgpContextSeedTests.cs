using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SGP.Infrastructure.Context;
using SGP.Tests.Constants;
using SGP.Tests.Fixtures;
using SGP.Tests.Mocks;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Infrastructure.Context
{
    [UnitTest(TestCategories.Infrastructure)]
    public class SgpContextSeedTests : IClassFixture<EfSqliteFixture>
    {
        private readonly EfSqliteFixture _fixture;

        public SgpContextSeedTests(EfSqliteFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_ReturnsRowsAffected_WhenEnsureSeedData()
        {
            // Arrange
            var logger = LoggerFactoryMock.Create();
            var context = _fixture.Context;

            // Act
            var actual = await context.EnsureSeedDataAsync(logger);
            var totalRegioes = await context.Regioes.AsNoTracking().CountAsync();
            var totalEstados = await context.Estados.AsNoTracking().CountAsync();
            var totalCidades = await context.Cidades.AsNoTracking().CountAsync();

            // Assert
            actual.Should().Be(totalRegioes + totalEstados + totalCidades);
            totalRegioes.Should().Be(Totais.Regioes);
            totalEstados.Should().Be(Totais.Estados);
            totalCidades.Should().Be(Totais.Cidades);
        }
    }
}