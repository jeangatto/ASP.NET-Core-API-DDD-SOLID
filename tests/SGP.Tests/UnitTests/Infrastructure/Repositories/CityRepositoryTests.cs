using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SGP.Domain.Entities;
using SGP.Infrastructure.Context;
using SGP.Infrastructure.Repositories;
using SGP.Shared.Extensions;
using SGP.Tests.Fixtures;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Infrastructure.Repositories
{
    [Category(TestCategories.Infrastructure)]
    public class CityRepositoryTests : IClassFixture<EfSqliteFixture>
    {
        private readonly EfSqliteFixture _fixture;

        public CityRepositoryTests(EfSqliteFixture fixture)
        {
            _fixture = fixture;
            SeedCities(_fixture.Context);
        }

        [Theory]
        [UnitTest]
        [InlineData("SP", 645)]
        [InlineData("RJ", 92)]
        [InlineData("DF", 1)]
        public async Task Should_ReturnsCities_WhenGetByExistingState(string state, int expectedCount)
        {
            // Arrange
            var repository = new CityRepository(_fixture.Context);

            // Act
            var act = await repository.GetAllCitiesAsync(state);

            // Assert
            act.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.HaveCount(expectedCount);
        }

        [Fact]
        [UnitTest]
        public async Task Should_ReturnsAllStates_WhenGetAllStates()
        {
            // Arrange
            var repository = new CityRepository(_fixture.Context);

            // Act
            var act = await repository.GetAllStatesAsync();

            // Assert
            act.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.BeInAscendingOrder()
                .And.HaveCount(27);
        }

        [Fact]
        [UnitTest]
        public async Task Should_ReturnsCity_WhenGetByExistingIbge()
        {
            // Arrange
            const string ibge = "3557105";
            var repository = new CityRepository(_fixture.Context);

            // Act
            var act = await repository.GetByIbgeAsync(ibge);

            // Assert
            act.Should().NotBeNull().And.BeEquivalentTo(new City(ibge, "SP", "Votuporanga"));
        }

        [Theory]
        [UnitTest]
        [InlineData("")]
        [InlineData("0")]
        [InlineData("00000")]
        [InlineData("ab2c3")]
        public async Task Should_ReturnsNull_WhenGetByInexistingIbge(string ibge)
        {
            // Arrange
            var repository = new CityRepository(_fixture.Context);

            // Act
            var act = await repository.GetByIbgeAsync(ibge);

            // Assert
            act.Should().BeNull();
        }

        private static void SeedCities(SgpContext context)
        {
            if (!context.Cities.AsNoTracking().Any())
            {
                var path = Path.Combine(SgpContextSeed.RootFolderPath, SgpContextSeed.SeedFolderName, "cities.json");
                var citiesJson = File.ReadAllText(path, Encoding.UTF8);
                context.Cities.AddRange(citiesJson.FromJson<IEnumerable<City>>());
                context.SaveChanges();
            }
        }
    }
}