using System;
using FluentAssertions;
using SGP.Infrastructure.Services;
using SGP.Shared.Interfaces;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Infrastructure.Services;

[UnitTest]
public class LocalDateTimeServiceTests
{
    [Fact]
    public void Sould_ReturnDateNow()
    {
        // Arrange
        var dateTimeService = CreateDateTime();

        // Act
        var actual = dateTimeService.Now;

        // Assert
        actual.Should().BeSameDateAs(DateTime.Now);
    }

    private static IDateTimeService CreateDateTime() => new DateTimeService();
}