using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SGP.Infrastructure.Context;
using SGP.Tests.Fixtures;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Infrastructure.Context
{
    [Category(TestCategories.Infrastructure)]
    public class SgpContextSeedTests : IClassFixture<EfSqliteFixture>
    {
        private readonly EfSqliteFixture _fixture;

        public SgpContextSeedTests(EfSqliteFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        [UnitTest]
        public async Task Should_SeedDataBase()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(s => s.CreateLogger(It.IsAny<string>())).Returns(loggerMock.Object);

            // Act
            await _fixture.Context.EnsureSeedDataAsync(loggerFactoryMock.Object);

            // Assert
            _fixture.Context.Cities.AsNoTracking().Count().Should().Be(5570);
        }
    }
}