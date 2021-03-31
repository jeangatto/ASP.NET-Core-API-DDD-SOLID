using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SGP.Infrastructure.Services;
using SGP.Shared.Interfaces;
using System;

namespace SGP.Infrastructure.Tests.Services
{
    [TestClass]
    [TestCategory("InfrastructureServices")]
    public class BCryptHashServiceTests
    {
        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void Compare_HashNullOrWhiteSpace_ThrowsArgumentException(string hash)
        {
            // Arrange
            IHashService hashService = CreateDefaultHashService();
            const string PASSWORD = "12345abc";

            // Act
            Action act = () => hashService.Compare(PASSWORD, hash);

            // Assert
            act.Should().Throw<ArgumentException>().And.ParamName.Should().Be("hash");
        }

        [TestMethod]
        public void Compare_Text_And_PreviouslyHashedText_ReturnsTrue()
        {
            // Arrange
            IHashService hashService = CreateDefaultHashService();
            const string PASSWORD = "12345abc";
            const string HASH = "$2a$11$pbVXrwtaofL9vV3FqhIU0esyCRj2iHHtSMvky/y.kcUaoQPQi7jiW";

            // Act
            bool act = hashService.Compare(PASSWORD, HASH);

            // Assert
            act.Should().BeTrue();
        }

        [TestMethod]
        public void Compare_Text_Diff_PreviouslyHashedText_ReturnsFalse()
        {
            // Arrange
            IHashService hashService = CreateDefaultHashService();
            const string PASSWORD = "abc12345";
            const string HASH = "$2a$11$pbVXrwtaofL9vV3FqhIU0esyCRj2iHHtSMvky/y.kcUaoQPQi7jiW";

            // Act
            bool act = hashService.Compare(PASSWORD, HASH);

            // Assert
            act.Should().BeFalse();
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void Compare_TextNullOrWhiteSpace_ThrowsArgumentException(string text)
        {
            // Arrange
            IHashService hashService = CreateDefaultHashService();
            const string HASH = "$2a$11$pbVXrwtaofL9vV3FqhIU0esyCRj2iHHtSMvky/y.kcUaoQPQi7jiW";

            // Act
            Action act = () => hashService.Compare(text, HASH);

            // Assert
            act.Should().Throw<ArgumentException>().And.ParamName.Should().Be("text");
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void Encrypt_InputNullOrWhiteSpace_ThrowsArgumentException(string input)
        {
            // Arrange
            IHashService hashService = CreateDefaultHashService();

            // Act
            Action act = () => hashService.Hash(input);

            // Assert
            act.Should().Throw<ArgumentException>().And.ParamName.Should().Be("text");
        }

        [TestMethod]
        [DataRow("a1b2c3d4")]
        [DataRow("MinhaSenha")]
        [DataRow("12345@__$Ááeeeiiooouu")]
        public void Encrypt_Text_ReturnsHashedString(string textToEncrypt)
        {
            // Arrange
            IHashService hashService = CreateDefaultHashService();

            // Act
            string act = hashService.Hash(textToEncrypt);

            // Assert
            act.Should().NotBeNullOrEmpty().And.Should().NotBeSameAs(textToEncrypt);
        }

        private static IHashService CreateDefaultHashService()
        {
            var services = new ServiceCollection();

            services.AddLogging(options => options.ClearProviders());

            var serviceProvider = services.BuildServiceProvider(true);

            var logger = serviceProvider.GetRequiredService<ILogger<BCryptHashService>>();

            return new BCryptHashService(logger);
        }
    }
}