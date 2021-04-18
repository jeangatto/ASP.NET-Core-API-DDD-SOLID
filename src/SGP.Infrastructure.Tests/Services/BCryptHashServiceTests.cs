using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SGP.Infrastructure.Services;
using SGP.Shared.Interfaces;
using System;
using Xunit;
using Xunit.Categories;

namespace SGP.Infrastructure.Tests.Services
{
    public class BCryptHashServiceTests
    {
        [Theory]
        [UnitTest]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Compare_HashNullOrWhiteSpace_ThrowsArgumentException(string hash)
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
        public void Compare_Text_And_PreviouslyHashedText_ReturnsTrue()
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
        public void Compare_Text_Diff_PreviouslyHashedText_ReturnsFalse()
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
        public void Compare_TextNullOrWhiteSpace_ThrowsArgumentException(string text)
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
        public void Encrypt_InputNullOrWhiteSpace_ThrowsArgumentException(string input)
        {
            // Arrange
            var hashService = CreateBCryptHashService();

            // Act
            Action act = () => hashService.Hash(input);

            // Assert
            act.Should().Throw<ArgumentException>().And.ParamName.Should().Be("text");
        }

        [Theory]
        [UnitTest]
        [InlineData("a1b2c3d4")]
        [InlineData("MinhaSenha")]
        [InlineData("12345@__$Ááeeeiiooouu")]
        public void Encrypt_Text_ReturnsHashedString(string textToEncrypt)
        {
            // Arrange
            var hashService = CreateBCryptHashService();

            // Act
            var act = hashService.Hash(textToEncrypt);

            // Assert
            act.Should().NotBeNullOrEmpty().And.Should().NotBeSameAs(textToEncrypt);
        }

        private static IHashService CreateBCryptHashService()
            => new BCryptHashService(Mock.Of<ILogger<BCryptHashService>>());
    }
}