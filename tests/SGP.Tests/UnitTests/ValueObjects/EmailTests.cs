using FluentAssertions;
using SGP.Domain.ValueObjects;
using SGP.Shared.Exceptions;
using SGP.Tests.Constants;
using System;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.ValueObjects
{
    [UnitTest(TestCategories.Domain)]
    public class EmailTests
    {
        [Theory]
        [ClassData(typeof(TestDatas.ValidEmailAddresses))]
        public void Should_ReturnsEmail_WhenEmailAddressIsValid(string emailAddress)
        {
            // Arrange
            var expectedEmailAddress = emailAddress.ToLowerInvariant();

            // Act
            var act = Email.Create(emailAddress);

            // Assert
            act.Should().NotBeNull();
            act.Address.Should().NotBeNullOrWhiteSpace().And.Be(expectedEmailAddress);
            act.ToString().Should().NotBeNullOrWhiteSpace().And.Be(expectedEmailAddress);
        }

        [Theory]
        [ClassData(typeof(TestDatas.InvalidEmailAddresses))]
        public void Should_ThrowsBusinessRuleException_WhenEmailIsInvalid(string emailAddress)
        {
            // Act
            Action act = () => Email.Create(emailAddress);

            // Assert
            act.Should().ThrowExactly<BusinessException>();
        }
    }
}
