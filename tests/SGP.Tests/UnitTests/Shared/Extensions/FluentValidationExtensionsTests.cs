using FluentValidation;
using FluentValidation.TestHelper;
using SGP.Shared.Extensions;
using SGP.Tests.Constants;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Shared.Extensions
{
    [UnitTest(TestCategories.Shared)]
    public class FluentValidationExtensionsTests
    {
        [Theory]
        [ClassData(typeof(TestDatas.ValidEmailAddresses))]
        public void Should_ReturnsSuccess_WhenEmailIsValid(string emailAddress)
        {
            // Arrange
            var validator = CreateValidator();

            // Act
            var result = validator.TestValidate(emailAddress);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [ClassData(typeof(TestDatas.InvalidEmailAddresses))]
        public void Should_ReturnsValidationError_WhenEmailIsInvalid(string emailAddress)
        {
            // Arrange
            var validator = CreateValidator();

            // Act
            var result = validator.TestValidate(emailAddress);

            // Assert
            result.ShouldHaveAnyValidationError();
        }

        private static IValidator<string> CreateValidator()
        {
            var validator = new InlineValidator<string>();
            validator.RuleFor(address => address).IsValidEmailAddress();
            return validator;
        }
    }
}