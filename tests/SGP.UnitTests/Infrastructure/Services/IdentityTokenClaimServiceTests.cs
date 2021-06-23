using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Options;
using SGP.Infrastructure.Services;
using SGP.Shared.AppSettings;
using SGP.Shared.Interfaces;
using SGP.SharedTests.Constants;
using System;
using System.Linq;
using System.Security.Claims;
using Xunit;
using Xunit.Categories;

namespace SGP.UnitTests.Infrastructure.Services
{
    [UnitTest(TestCategories.Infrastructure)]
    public class IdentityTokenClaimServiceTests
    {
        private static readonly Faker Faker = new();

        [Fact]
        public void Should_ReturnsAcessToken_WhenGenerateAccessTokenWithValidClaims()
        {
            // Arrange
            var service = CreateTokenClaimsService();
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, Faker.Person.UserName),
                new Claim(ClaimTypes.Email, Faker.Person.Email),
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
            Action act = () => service.GenerateAccessToken(Enumerable.Empty<Claim>());

            // Assert
            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void Should_ThrowsArgumentNullException_WhenGenerateAccessTokenWithNullClaims()
        {
            // Arrange
            var service = CreateTokenClaimsService();

            // Act
            Action act = () => service.GenerateAccessToken(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        private static IDateTime CreateDateTimeService()
            => new LocalDateTimeService();

        private static IOptions<JwtConfig> CreateJwtConfigOptions()
            => Options.Create(JwtConfig.Create("Clients-API-SGP", "API-SGP", 21600, "mgnCsPC22aPqn7YWb7rn2FEtqsJW9Apv", true, true));

        private static ITokenClaimsService CreateTokenClaimsService()
            => new IdentityTokenClaimService(CreateJwtConfigOptions(), CreateDateTimeService());
    }
}
