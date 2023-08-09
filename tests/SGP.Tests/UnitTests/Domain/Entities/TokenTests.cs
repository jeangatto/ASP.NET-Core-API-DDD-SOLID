using System;
using Bogus;
using FluentAssertions;
using NSubstitute;
using SGP.Domain.Entities;
using SGP.Shared.Abstractions;
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

        // Act
        var act = token.EstaValido(Substitute.For<IDateTimeService>());

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
        var dateTimeService = Substitute.For<IDateTimeService>();
        dateTimeService.Now.Returns(DateTime.Now.AddDays(8));

        // Act
        var act = token.EstaValido(dateTimeService);

        // Assert
        act.Should().BeFalse();
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