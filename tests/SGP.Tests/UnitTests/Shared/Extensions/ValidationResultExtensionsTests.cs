using FluentAssertions;
using FluentResults.Extensions.FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using SGP.Shared.Extensions;
using SGP.Tests.Extensions;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Shared.Extensions
{
    [Category(TestCategories.Shared)]
    public class ValidationResultExtensionsTests : UnitTestBase
    {
        private static readonly string[] MetadataKeys = new[] { "PropertyName", "AttemptedValue", "ErrorCode" };

        [Fact]
        public void Should_ReturnResultWithErrors_WhenValidationFail()
        {
            // Arrange
            var user = new User("ma...ma@jjf.co");
            var validationResult = CreateValidatorAndValid(user);

            // Act
            var actual = validationResult.ToFail();

            // Assert
            actual.Should().BeFailure()
                .And.Subject.Errors.Should().HaveCountGreaterThan(0)
                .And.Subject.ForEach(error =>
                {
                    error.Message.Should().NotBeNullOrEmpty();
                    error.Metadata.Should().NotBeEmpty()
                        .And.HaveCount(MetadataKeys.Length)
                        .And.ContainKeys(MetadataKeys);
                });
        }

        [Fact]
        public void Should_ReturnResultWithNoErrors_WhenValidationPass()
        {
            // Arrange
            var user = new User("john.doe@hotmail.com");
            var validationResult = CreateValidatorAndValid(user);

            // Act
            var actual = validationResult.ToFail();

            // Assert
            actual.Should().BeSuccess();
        }

        [Fact]
        public void Should_ReturnTypedResultWithErrors_WhenValidationFail()
        {
            // Arrange
            var user = new User(string.Empty);
            var validationResult = CreateValidatorAndValid(user);

            // Act
            var actual = validationResult.ToFail<User>();

            // Assert
            actual.Should().BeFailure()
                .And.Subject.Errors.Should().HaveCountGreaterThan(0)
                .And.Subject.ForEach(error =>
                {
                    error.Message.Should().NotBeNullOrEmpty();
                    error.Metadata.Should().NotBeEmpty()
                        .And.HaveCount(MetadataKeys.Length)
                        .And.ContainKeys(MetadataKeys);
                });
        }

        private static ValidationResult CreateValidatorAndValid(User user)
            => new UserValidator().Validate(user);

        private class User
        {
            public User(string email) => Email = email;

            public string Email { get; private init; }
        }

        private class UserValidator : AbstractValidator<User>
        {
            public UserValidator() => RuleFor(x => x.Email).NotEmpty().IsValidEmailAddress();
        }
    }
}