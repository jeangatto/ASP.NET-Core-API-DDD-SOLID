using FluentAssertions;
using SGP.Domain.Entities;
using SGP.Infrastructure.Repositories;
using SGP.Tests.Fixtures;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Infrastructure.Repositories
{
    [Category(TestCategories.Infrastructure)]
    public class CidadeRepositoryTests : IClassFixture<EfFixture>
    {
        private readonly EfFixture _fixture;

        public CidadeRepositoryTests(EfFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [UnitTest]
        [InlineData("SP", 645)]
        [InlineData("RJ", 92)]
        [InlineData("DF", 1)]
        public async Task Should_ReturnsCities_WhenGetByExistingState(string state, int expectedCount)
        {
            // Arrange
            var cidadeRepository = new CidadeRepository(_fixture.Context);

            // Act
            var act = await cidadeRepository.GetAllAsync(state);

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
            var cidadeRepository = new CidadeRepository(_fixture.Context);

            // Act
            var act = await cidadeRepository.GetAllEstadosAsync();

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
            var expected = new Cidade("3557105", "SP", "Votuporanga");
            var cidadeRepository = new CidadeRepository(_fixture.Context);

            // Act
            var act = await cidadeRepository.GetByIbgeAsync(expected.Ibge);

            // Assert
            act.Should().NotBeNull().And.BeEquivalentTo(expected);
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
            var cidadeRepository = new CidadeRepository(_fixture.Context);

            // Act
            var act = await cidadeRepository.GetByIbgeAsync(ibge);

            // Assert
            act.Should().BeNull();
        }
    }
}