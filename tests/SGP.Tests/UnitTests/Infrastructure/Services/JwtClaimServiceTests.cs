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

namespace SGP.Tests.UnitTests.Infrastructure.Services;

[UnitTest]
public class JwtClaimServiceTests
{
    [Fact]
    public void Should_ReturnsAcessToken_WhenGenerateAccessTokenWithValidClaims()
    {
        // Arrange
        var faker = new Faker().Person;
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, faker.UserName, ClaimValueTypes.String),
            new Claim(ClaimTypes.Email, faker.Email, ClaimValueTypes.Email)
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
        => new JwtClaimService(CreateJwtConfig(), CreateDateTimeService());

    private static IDateTimeService CreateDateTimeService()
        => new DateTimeService();

    private static IOptions<JwtOptions> CreateJwtConfig()
    {
        const string audience = "Clients-API-SGP";
        const string issuer = "API-SGP";
        const string secretKey = "p8SXNddEAEn1cCuyfVJKYA7e6hlagbLd";
        const short seconds = 21600;
        var jwtConfig = JwtOptions.Create(audience, issuer, seconds, secretKey);
        return Options.Create(jwtConfig);
    }
}