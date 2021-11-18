using System;
using System.Security.Claims;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Options;
using SGP.Infrastructure.Services;
using SGP.Shared.AppSettings;
using SGP.Shared.Interfaces;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Infrastructure.Services
{
    [UnitTest]
    public class IdentityTokenClaimServiceTests
    {
        [Fact]
        public void Should_ReturnsAcessToken_WhenGenerateAccessTokenWithValidClaims()
        {
            // Arrange
            var faker = new Faker();
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, faker.Internet.UserName(), ClaimValueTypes.String),
                new Claim(ClaimTypes.Email, faker.Internet.Email(), ClaimValueTypes.Email)
            };
            var service = CreateTokenClaimsService();

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
            Action actual = () => service.GenerateAccessToken(Array.Empty<Claim>());

            // Assert
            actual.Should().ThrowExactly<ArgumentException>().And.ParamName.Should().Be("claims");
        }

        [Fact]
        public void Should_ThrowsArgumentNullException_WhenGenerateAccessTokenWithNullClaims()
        {
            // Arrange
            var service = CreateTokenClaimsService();

            // Act
            Action actual = () => service.GenerateAccessToken(null);

            // Assert
            actual.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("claims");
        }

        private static ITokenClaimsService CreateTokenClaimsService()
            => new IdentityTokenClaimService(CreateJwtConfig(), CreateDateTimeService());

        private static IDateTime CreateDateTimeService()
            => new LocalDateTimeService();

        private static IOptions<JwtConfig> CreateJwtConfig()
        {
            const string audience = "Clients-API-SGP";
            const string issuer = "API-SGP";
            const string secretKey = "p8SXNddEAEn1cCuyfVJKYA7e6hlagbLd";
            const short seconds = 21600;
            const bool validateAudience = true;
            const bool validateIssuer = true;
            var jwtConfig = JwtConfig.Create(audience, issuer, seconds, secretKey, validateAudience, validateIssuer);
            return Options.Create(jwtConfig);
        }
    }
}