using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SGP.Infrastructure.Context;
using SGP.Tests.Constants;
using SGP.Tests.Fixtures;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Infrastructure.Context
{
    [UnitTest]
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
            var actual = await context.EnsureSeedDataAsync();
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