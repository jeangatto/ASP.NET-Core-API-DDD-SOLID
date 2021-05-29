using FluentAssertions;
using SGP.Infrastructure.Services;
using SGP.Shared.Interfaces;
using System;
using Xunit;

namespace SGP.Tests.UnitTests.Infrastructure.Services
{
    public class LocalDateTimeServiceTests : UnitTestBase
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

        private static IDateTime CreateDateTime() => new LocalDateTimeService();
    }
}