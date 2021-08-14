using System;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SGP.Infrastructure.Services;
using SGP.Shared.Interfaces;
using SGP.Tests.Constants;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Infrastructure.Services
{
    [UnitTest(TestCategories.Infrastructure)]
    public class BCryptHashServiceTests
    {
        [Fact]
        public void Should_ReturnsFalse_WhenCompareTextDiffPreviouslyHashedText()
        {
            // Arrange
            var hashService = CreateHashService();
            const string password = "abc12345";
            const string hash = "$2a$11$pbVXrwtaofL9vV3FqhIU0esyCRj2iHHtSMvky/y.kcUaoQPQi7jiW";

            // Act
            var actual = hashService.Compare(password, hash);

            // Assert
            actual.Should().BeFalse();
        }

        [Theory]
        [InlineData("a1b2c3d4")]
        [InlineData("AB12345")]
        [InlineData("MinhaSenha")]
        [InlineData("12345@__$Ááeeeiiooouu")]
        public void Should_ReturnsHashedString_WhenHashTextIsValid(string text)
        {
            // Arrange
            var hashService = CreateHashService();

            // Act
            var actual = hashService.Hash(text);

            // Assert
            actual.Should().NotBeNullOrEmpty().And.Should().NotBeSameAs(text);
        }

        [Fact]
        public void Should_ReturnsTrue_WhenCompareTextAndPreviouslyHashedText()
        {
            // Arrange
            var hashService = CreateHashService();
            const string password = "12345abc";
            const string hash = "$2a$11$pbVXrwtaofL9vV3FqhIU0esyCRj2iHHtSMvky/y.kcUaoQPQi7jiW";

            // Act
            var actual = hashService.Compare(password, hash);

            // Assert
            actual.Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("")]
        public void Should_ThrowsArgumentException_WhenCompareHashIsInvalid(string hash)
        {
            // Arrange
            var hashService = CreateHashService();
            const string password = "12345abc";

            // Act
            Action actual = () => hashService.Compare(password, hash);

            // Assert
            actual.Should().Throw<ArgumentException>().And.ParamName.Should().Be("hash");
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("")]
        public void Should_ThrowsArgumentException_WhenCompareTextIsInvalid(string text)
        {
            // Arrange
            var hashService = CreateHashService();
            const string hash = "$2a$11$pbVXrwtaofL9vV3FqhIU0esyCRj2iHHtSMvky/y.kcUaoQPQi7jiW";

            // Act
            Action actual = () => hashService.Compare(text, hash);

            // Assert
            actual.Should().Throw<ArgumentException>().And.ParamName.Should().Be("text");
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("")]
        public void Should_ThrowsArgumentException_WhenHashTextIsInvalid(string text)
        {
            // Arrange
            var hashService = CreateHashService();

            // Act
            Action actual = () => hashService.Hash(text);

            // Assert
            actual.Should().Throw<ArgumentException>().And.ParamName.Should().Be("text");
        }

        private static IHashService CreateHashService() => new BCryptHashService(Mock.Of<ILogger<BCryptHashService>>());
    }
}