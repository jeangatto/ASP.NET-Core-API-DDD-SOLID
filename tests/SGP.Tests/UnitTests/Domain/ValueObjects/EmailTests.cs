using FluentAssertions;
using SGP.Domain.ValueObjects;
using Xunit;

namespace SGP.Tests.UnitTests.Domain.ValueObjects
{
    public class EmailTests
    {
        [Fact]
        public void Should_TrimAndLowerAddress_WhenInstantiateEmail()
        {
            // Arrange
            const string emailAddress = "  john.doe@HOTMAIL.COM   ";
            const string expected = "john.doe@hotmail.com";

            // Act
            var actual = new Email(emailAddress);

            // Assert
            actual.Should().NotBeNull();
            actual.Address.Should().NotBeNullOrWhiteSpace().And.Be(expected).And.NotBeUpperCased();
        }
    }
}