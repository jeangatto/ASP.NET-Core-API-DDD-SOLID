using System;
using System.Security.Claims;
using FluentAssertions;
using SGP.Infrastructure.Services;
using SGP.Shared.Interfaces;
using SGP.Tests.Constants;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Infrastructure.Services
{
    [UnitTest(TestCategories.Infrastructure)]
    public class IdentityTokenClaimServiceTests : UnitTestBase
    {
        [Fact]
        public void Should_ReturnsAcessToken_WhenGenerateAccessTokenWithValidClaims()
        {
            // Arrange
            var service = CreateTokenClaimsService();
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, Faker.Random.Guid().ToString()),
                new Claim(ClaimTypes.Name, Faker.Person.UserName), new Claim(ClaimTypes.Email, Faker.Person.Email)
            };

            // Act
            var actual = service.GenerateAccessToken(claims);

            // Assert
            actual.Should().NotBeNull();
            actual.Token.Should().NotBeNullOrWhiteSpace();
            actual.ExpiresAt.Should().BeAfter(actual.CreatedAt);
        }

        [Fact]
        public void Should_ReturnseRefreshToken_WhenGenerateRefreshToken()
        {
            // Arrange
            var service = CreateTokenClaimsService();

            // Act
            var actual = service.GenerateRefreshToken();

            // Assert
            actual.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void Should_ThrowsArgumentException_WhenGenerateAccessTokenWithEmptyClaims()
        {
            // Arrange
            var service = CreateTokenClaimsService();

            // Act
            Action act = () => service.GenerateAccessToken(Array.Empty<Claim>());

            // Assert
            act.Should().ThrowExactly<ArgumentException>().And.ParamName.Should().Be("claims");
        }

        [Fact]
        public void Should_ThrowsArgumentNullException_WhenGenerateAccessTokenWithNullClaims()
        {
            // Arrange
            var service = CreateTokenClaimsService();

            // Act
            Action act = () => service.GenerateAccessToken(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("claims");
        }

        private static ITokenClaimsService CreateTokenClaimsService()
        {
            var dateTime = new LocalDateTimeService();
            return new IdentityTokenClaimService(CreateJwtConfigOptions(), dateTime);
        }
    }
}