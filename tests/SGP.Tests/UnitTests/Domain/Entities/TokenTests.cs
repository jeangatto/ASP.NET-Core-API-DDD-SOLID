using System;
using Bogus;
using FluentAssertions;
using Moq;
using SGP.Domain.Entities;
using SGP.Infrastructure.Services;
using SGP.Shared.Interfaces;
using SGP.Tests.Extensions;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Domain.Entities;

[UnitTest]
public class TokenTests
{
    [Fact]
    public void Devera_RetornarVerdadeiro_QuandoTokenEstiverValido()
    {
        // Arrange
        var faker = new Faker();
        var accessToken = faker.Random.JsonWebToken();
        var refreshToken = faker.Random.JsonWebToken();
        var criadoEm = DateTime.Now;
        var expiraEm = criadoEm.AddDays(7);
        var token = new Token(accessToken, refreshToken, criadoEm, expiraEm);
        var dateTimeService = new DateTimeService();

        // Act
        var act = token.EstaValido(dateTimeService);

        // Assert
        act.Should().BeTrue();
    }

    [Fact]
    public void Devera_RetornarFalso_QuandoTokenNaoEstiverValido()
    {
        // Arrange
        var faker = new Faker();
        var accessToken = faker.Random.JsonWebToken();
        var refreshToken = faker.Random.JsonWebToken();
        var criadoEm = DateTime.Now;
        var expiraEm = criadoEm.AddDays(7);
        var token = new Token(accessToken, refreshToken, criadoEm, expiraEm);
        var dateTimeMock = new Mock<IDateTimeService>();
        dateTimeMock.Setup(s => s.Now).Returns(DateTime.Now.AddDays(8));

        // Act
        var act = token.EstaValido(dateTimeMock.Object);

        // Assert
        act.Should().BeFalse();
        dateTimeMock.Verify(s => s.Now, Times.Once);
    }

    [Fact]
    public void Devera_RevogarToken()
    {
        // Arrange
        var faker = new Faker();
        var accessToken = faker.Random.JsonWebToken();
        var refreshToken = faker.Random.JsonWebToken();
        var criadoEm = DateTime.Now;
        var expiraEm = criadoEm.AddDays(7);
        var token = new Token(accessToken, refreshToken, criadoEm, expiraEm);
        var dataRevogacao = criadoEm.AddDays(3);

        // Act
        token.Revogar(dataRevogacao);

        // Assert
        token.EstaRevogado.Should().BeTrue();
        token.RevogadoEm.Should().NotBeNull().And.Be(dataRevogacao);
    }
}