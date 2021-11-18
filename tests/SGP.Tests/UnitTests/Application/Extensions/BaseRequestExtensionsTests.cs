using FluentAssertions;
using SGP.Application.Requests.CidadeRequests;
using SGP.Application.Responses;
using SGP.Shared.Errors;
using SGP.Shared.Extensions;
using SGP.Tests.Extensions;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Application.Extensions
{
    [UnitTest]
    public class BaseRequestExtensionsTests
    {
        [Fact]
        public void Should_ReturnResultTypedWithErrors_WhenValidationFail()
        {
            // Arrange
            var request = new ObterTodosPorUfRequest(string.Empty);
            request.Validate();

            // Act
            var actual = request.ToFail<CidadeResponse>();

            // Assert
            actual.Should().NotBeNull();
            actual.HasError<ValidationError>().Should().BeTrue();
            actual.IsFailed.Should().BeTrue();
            actual.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.Subject.ForEach(error => error.Message.Should().NotBeNullOrEmpty());
        }

        [Fact]
        public void Should_ReturnResultWithErrors_WhenValidationFail()
        {
            // Arrange
            var request = new ObterTodosPorUfRequest(string.Empty);
            request.Validate();

            // Act
            var actual = request.ToFail();

            // Assert
            actual.Should().NotBeNull();
            actual.HasError<ValidationError>().Should().BeTrue();
            actual.IsFailed.Should().BeTrue();
            actual.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
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
            actual.Should().NotBeNull();
            actual.IsSuccess.Should().BeTrue();
        }
    }
}