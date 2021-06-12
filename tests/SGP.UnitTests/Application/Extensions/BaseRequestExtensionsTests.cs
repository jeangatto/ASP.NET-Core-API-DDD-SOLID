using FluentAssertions;
using FluentResults.Extensions.FluentAssertions;
using FluentValidation;
using SGP.Application.Requests.CidadeRequests;
using SGP.Application.Responses;
using SGP.Shared.Errors;
using SGP.Shared.Extensions;
using SGP.SharedTests;
using SGP.SharedTests.Extensions;
using Xunit;
using Xunit.Categories;

namespace SGP.UnitTests.Application.Extensions
{
    [UnitTest(TestCategories.Shared)]
    public class BaseRequestExtensionsTests
    {
        [Fact]
        public void Should_ReturnResultTypedWithErrors_WhenValidationFail()
        {
            // Arrange
            var request = new ObterTodosPorUfRequest("");
            request.Validate();

            // Act
            var actual = request.ToFail<CidadeResponse>();

            // Assert
            actual.HasError<ValidationError>().Should().BeTrue();
            actual.Should().BeFailure()
                .And.Subject.Errors.Should().HaveCountGreaterThan(0)
                .And.Subject.ForEach(error => error.Message.Should().NotBeNullOrEmpty());
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
            actual.HasError<ValidationError>().Should().BeTrue();
            actual.Should().BeFailure()
                .And.Subject.Errors.Should().HaveCountGreaterThan(0)
                .And.Subject.ForEach(error => error.Message.Should().NotBeNullOrEmpty());
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