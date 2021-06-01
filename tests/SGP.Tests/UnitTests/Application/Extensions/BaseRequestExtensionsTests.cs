using FluentAssertions;
using FluentResults.Extensions.FluentAssertions;
using FluentValidation;
using SGP.Application.Extensions;
using SGP.Application.Requests.CidadeRequests;
using SGP.Application.Responses;
using SGP.Tests.Extensions;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Shared.Extensions
{
    [UnitTest(TestCategories.Shared)]
    public class ValidationResultExtensionsTests
    {
        private static readonly string[] MetadataKeys = new[] { "PropertyName", "AttemptedValue", "ErrorCode" };

        [Fact]
        public void Should_ReturnResultTypedWithErrors_WhenValidationFail()
        {
            // Arrange
            var request = new ObterTodosPorUfRequest("");
            request.Validate();

            // Act
            var actual = request.ToFail<CidadeResponse>();

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
        public void Should_ReturnResultWithErrors_WhenValidationFail()
        {
            // Arrange
            var request = new ObterTodosPorUfRequest("");
            request.Validate();

            // Act
            var actual = request.ToFail();

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
            var request = new ObterTodosPorUfRequest("SP");
            request.Validate();

            // Act
            var actual = request.ToFail();

            // Assert
            actual.Should().BeSuccess();
        }
    }
}