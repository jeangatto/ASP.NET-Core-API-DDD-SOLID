using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SGP.Infrastructure.Data.Context;
using SGP.Tests.Constants;
using SGP.Tests.Fixtures;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Infrastructure.Data.Context;

[UnitTest]
public class SgpContextSeedTests(EfSqliteFixture fixture) : IClassFixture<EfSqliteFixture>
{
    [Fact]
    public async Task Should_ReturnsRowsAffected_WhenEnsureSeedData()
    {
        // Arrange
        var context = fixture.Context;

        // Act
        await context.EnsureSeedDataAsync();
        var totalRegioes = await context.Regioes.AsNoTracking().CountAsync();
        var totalEstados = await context.Estados.AsNoTracking().CountAsync();
        var totalCidades = await context.Cidades.AsNoTracking().CountAsync();
        var totalInserido = totalRegioes + totalEstados + totalCidades;

        // Assert
        totalRegioes.Should().Be(Totais.Regioes);
        totalEstados.Should().Be(Totais.Estados);
        totalCidades.Should().Be(Totais.Cidades);
        totalInserido.Should().Be(totalRegioes + totalEstados + totalCidades);
    }
}