using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SGP.Infrastructure.Services;
using SGP.Shared.Interfaces;
using System;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Infrastructure.Services
{
    [Category(TestCategories.Infrastructure)]
    public class BCryptHashServiceTests
    {
        [Theory]
        [UnitTest]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_ThrowsArgumentException_WhenCompareHashIsInvalid(string hash)
        {
            // Arrange
            var hashService = CreateBCryptHashService();
            const string password = "12345abc";

            // Act
            Action act = () => hashService.Compare(password, hash);

            // Assert
            act.Should().Throw<ArgumentException>().And.ParamName.Should().Be("hash");
        }

        [Fact]
        [UnitTest]
        public void Should_ReturnsTrue_WhenCompareTextAndPreviouslyHashedText()
        {
            // Arrange
            var hashService = CreateBCryptHashService();
            const string password = "12345abc";
            const string hash = "$2a$11$pbVXrwtaofL9vV3FqhIU0esyCRj2iHHtSMvky/y.kcUaoQPQi7jiW";

            // Act
            var act = hashService.Compare(password, hash);

            // Assert
            act.Should().BeTrue();
        }

        [Fact]
        [UnitTest]
        public void Should_ReturnsFalse_WhenCompareTextDiffPreviouslyHashedText()
        {
            // Arrange
            var hashService = CreateBCryptHashService();
            const string password = "abc12345";
            const string hash = "$2a$11$pbVXrwtaofL9vV3FqhIU0esyCRj2iHHtSMvky/y.kcUaoQPQi7jiW";

            // Act
            var act = hashService.Compare(password, hash);

            // Assert
            act.Should().BeFalse();
        }

        [Theory]
        [UnitTest]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_ThrowsArgumentException_WhenCompareTextIsInvalid(string text)
        {
            // Arrange
            var hashService = CreateBCryptHashService();
            const string hash = "$2a$11$pbVXrwtaofL9vV3FqhIU0esyCRj2iHHtSMvky/y.kcUaoQPQi7jiW";

            // Act
            Action act = () => hashService.Compare(text, hash);

            // Assert
            act.Should().Throw<ArgumentException>().And.ParamName.Should().Be("text");
        }

        [Theory]
        [UnitTest]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_ThrowsArgumentException_WhenHashTextIsInvalid(string text)
        {
            // Arrange
            var hashService = CreateBCryptHashService();

            // Act
            Action act = () => hashService.Hash(text);

            // Assert
            act.Should().Throw<ArgumentException>().And.ParamName.Should().Be("text");
        }

        [Theory]
        [UnitTest]
        [Category(TestCategories.Infrastructure)]
        [InlineData("a1b2c3d4")]
        [InlineData("AB12345")]
        [InlineData("MinhaSenha")]
        [InlineData("12345@__$Ááeeeiiooouu")]
        public void Should_ReturnsHashedString_WhenHashTextIsValid(string text)
        {
            // Arrange
            var hashService = CreateBCryptHashService();

            // Act
            var act = hashService.Hash(text);

            // Assert
            act.Should().NotBeNullOrEmpty().And.Should().NotBeSameAs(text);
        }

        private static IHashService CreateBCryptHashService()
        {
            return new BCryptHashService(Mock.Of<ILogger<BCryptHashService>>());
        }
    }
}