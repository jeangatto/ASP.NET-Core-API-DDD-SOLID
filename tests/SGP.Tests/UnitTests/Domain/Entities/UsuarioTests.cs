using System;
using Bogus;
using FluentAssertions;
using SGP.Domain.Entities;
using SGP.Domain.ValueObjects;
using SGP.Infrastructure.Services;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Domain.Entities;

[UnitTest]
public class UsuarioTests
{
    [Fact]
    public void Devera_RetornarBloqueioVerdadeiro_QuandoIncrementarFalhasAoLimite()
    {
        // Arrange
        var faker = new Faker();
        var usuario = new Usuario(faker.Person.FullName, new Email(faker.Person.Email), faker.Internet.Password());
        var dateTimeService = new LocalDateTimeService();
        const int numeroTentativas = 1;
        usuario.IncrementarFalhas(dateTimeService, numeroTentativas, TimeSpan.FromMinutes(30));

        // Act
        var act = usuario.EstaBloqueado(dateTimeService);

        // Assert
        act.Should().BeTrue();
        usuario.BloqueioExpiraEm.Should().NotBeNull().And.BeSameDateAs(DateTime.Now);
        usuario.NumeroFalhasAoAcessar.Should().Be(0);
    }

    [Fact]
    public void Devera_RetornarBloqueioFalso_AoIncrementarFalhas()
    {
        // Arrange
        var faker = new Faker();
        var usuario = new Usuario(faker.Person.FullName, new Email(faker.Person.Email), faker.Internet.Password());
        var dateTimeService = new LocalDateTimeService();
        const int numeroTentativas = 3;
        usuario.IncrementarFalhas(dateTimeService, numeroTentativas, TimeSpan.FromMinutes(30));

        // Act
        var act = usuario.EstaBloqueado(dateTimeService);

        // Assert
        act.Should().BeFalse();
        usuario.BloqueioExpiraEm.Should().BeNull();
        usuario.NumeroFalhasAoAcessar.Should().Be(1);
    }
}