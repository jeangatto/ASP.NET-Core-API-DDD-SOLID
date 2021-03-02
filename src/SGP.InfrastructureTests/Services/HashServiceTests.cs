using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SGP.Shared.Interfaces;
using System;
using Xunit;

namespace SGP.Infrastructure.Services.Tests
{
    public class HashServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Compare_HashNullOrWhiteSpace_ThrowsArgumentException(string hash)
        {
            // Arrange
            var hashService = CreateDefaultHashService();
            const string PASSWORD = "12345abc";

            // Act
            Action actual = () => hashService.Compare(PASSWORD, hash);

            // Assert
            actual.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Compare_Text_And_PreviouslyHashedText_ReturnsTrue()
        {
            // Arrange
            var hashService = CreateDefaultHashService();
            const string PASSWORD = "12345abc";
            const string HASH = "$2a$11$pbVXrwtaofL9vV3FqhIU0esyCRj2iHHtSMvky/y.kcUaoQPQi7jiW";

            // Act
            var actual = hashService.Compare(PASSWORD, HASH);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void Compare_Text_Diff_PreviouslyHashedText_ReturnsFalse()
        {
            // Arrange
            var hashService = CreateDefaultHashService();
            const string PASSWORD = "abc12345";
            const string HASH = "$2a$11$pbVXrwtaofL9vV3FqhIU0esyCRj2iHHtSMvky/y.kcUaoQPQi7jiW";

            // Act
            var actual = hashService.Compare(PASSWORD, HASH);

            // Assert
            actual.Should().BeFalse();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Compare_TextNullOrWhiteSpace_ThrowsArgumentException(string text)
        {
            // Arrange
            var hashService = CreateDefaultHashService();
            const string HASH = "$2a$11$pbVXrwtaofL9vV3FqhIU0esyCRj2iHHtSMvky/y.kcUaoQPQi7jiW";

            // Act
            Action actual = () => hashService.Compare(text, HASH);

            // Assert
            actual.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Encrypt_InputNullOrWhiteSpace_ThrowsArgumentException(string input)
        {
            // Arrange
            var hashService = CreateDefaultHashService();

            // Act
            Action actual = () => hashService.Hash(input);

            // Assert
            actual.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Encrypt_Text_ReturnsHashedString()
        {
            // Arrange
            var hashService = CreateDefaultHashService();
            const string PASSWORD = "12345abc";

            // Act
            var actual = hashService.Hash(PASSWORD);

            // Assert
            actual.Should().NotBeNullOrWhiteSpace().And.NotBeEquivalentTo(PASSWORD);
        }

        private static IHashService CreateDefaultHashService()
        {
            var logger = Mock.Of<ILogger<HashService>>();
            return new HashService(logger);
        }
    }
}