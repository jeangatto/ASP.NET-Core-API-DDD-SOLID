using Ardalis.Result;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.PublicApi.Extensions;
using SGP.PublicApi.ObjectResults;
using SGP.Tests.Extensions;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.PublicApi.Extensions;

[UnitTest]
public class FluentResultExtensionsTests
{
    [Fact]
    public void Should_ReturnsBadRequestResult_WhenResultHasError()
    {
        // Arrange
        const int expectedStatusCode = StatusCodes.Status400BadRequest;
        const string errorMessage = "Requisição inválida.";

        // Act
        var actual = Result.Error(errorMessage).ToActionResult();

        // Assert
        actual.Should().NotBeNull().And.BeOfType<BadRequestObjectResult>();
        var apiResponse = actual.ToApiResponse();
        apiResponse.Should().NotBeNull();
        apiResponse.Success.Should().BeFalse();
        apiResponse.StatusCode.Should().Be(expectedStatusCode);
        apiResponse.Errors.Should().NotBeNullOrEmpty()
            .And.OnlyHaveUniqueItems()
            .And.ContainSingle()
            .And.Subject.ForEach(error => error.Message.Should().NotBeNullOrWhiteSpace().And.Be(errorMessage));
    }

    [Fact]
    public void Should_ReturnsBadRequestResult_WhenTypedResultHasError()
    {
        // Arrange
        const int expectedStatusCode = StatusCodes.Status400BadRequest;
        const string errorMessage = "Requisição inválida.";

        // Act
        var actual = Result<string>.Error(errorMessage).ToActionResult();

        // Assert
        actual.Should().NotBeNull().And.BeOfType<BadRequestObjectResult>();
        var apiResponse = actual.ToApiResponse();
        apiResponse.Should().NotBeNull();
        apiResponse.Success.Should().BeFalse();
        apiResponse.StatusCode.Should().Be(expectedStatusCode);
        apiResponse.Errors.Should().NotBeNullOrEmpty()
            .And.OnlyHaveUniqueItems()
            .And.ContainSingle()
            .And.Subject.ForEach(error => error.Message.Should().NotBeNullOrWhiteSpace().And.Be(errorMessage));
    }

    [Fact]
    public void Should_ReturnsBadRequestResult_WhenHasValidationErrors()
    {
        // Arrange
        const int expectedStatusCode = StatusCodes.Status400BadRequest;
        var validationErrors = new Faker<ValidationError>()
            .RuleFor(v => v.ErrorCode, f => f.Random.Number().ToString())
            .RuleFor(v => v.ErrorMessage, f => f.Random.String2(10))
            .Generate(10);

        // Act
        var actual = Result.Invalid(validationErrors).ToActionResult();

        // Assert
        actual.Should().NotBeNull().And.BeOfType<BadRequestObjectResult>();
        var apiResponse = actual.ToApiResponse();
        apiResponse.Should().NotBeNull();
        apiResponse.Success.Should().BeFalse();
        apiResponse.StatusCode.Should().Be(expectedStatusCode);
        apiResponse.Errors.Should().NotBeNullOrEmpty()
            .And.OnlyHaveUniqueItems()
            .And.HaveCount(validationErrors.Count)
            .And.Subject.ForEach(error => error.Message.Should().NotBeNullOrWhiteSpace());
    }

    [Fact]
    public void Should_ReturnsBadRequestResult_WhenResultHasMultipleErrors()
    {
        // Arrange
        const int expectedStatusCode = StatusCodes.Status400BadRequest;
        var errors = new[] { "Erro0", "Erro1", "Erro3", "Erro2" };

        // Act
        var actual = Result.Error(new ErrorList(errors)).ToActionResult();

        // Assert
        actual.Should().NotBeNull().And.BeOfType<BadRequestObjectResult>();
        var apiResponse = actual.ToApiResponse();
        apiResponse.Should().NotBeNull();
        apiResponse.Success.Should().BeFalse();
        apiResponse.StatusCode.Should().Be(expectedStatusCode);
        apiResponse.Errors.Should().NotBeNullOrEmpty()
            .And.OnlyHaveUniqueItems()
            .And.HaveCount(errors.Length)
            .And.Subject.ForEach(error => error.Message.Should().NotBeNullOrWhiteSpace());
    }

    [Fact]
    public void Should_ReturnsNotFoundRequestResult_WhenResultHasNotFoundError()
    {
        // Arrange
        const int expectedStatusCode = StatusCodes.Status404NotFound;
        const string errorMessage = "Nenhum registro encontrado.";

        // Act
        var actual = Result.NotFound(errorMessage).ToActionResult();

        // Assert
        actual.Should().NotBeNull().And.BeOfType<NotFoundObjectResult>();
        var apiResponse = actual.ToApiResponse();
        apiResponse.Should().NotBeNull();
        apiResponse.Success.Should().BeFalse();
        apiResponse.StatusCode.Should().Be(expectedStatusCode);
        apiResponse.Errors.Should().NotBeNullOrEmpty()
            .And.OnlyHaveUniqueItems()
            .And.ContainSingle()
            .And.Subject.ForEach(error => error.Message.Should().NotBeNullOrWhiteSpace().And.Be(errorMessage));
    }

    [Fact]
    public void Should_ReturnsUnauthorizedObjectResult_WhenResultHasNotFoundError()
    {
        // Arrange
        const int expectedStatusCode = StatusCodes.Status401Unauthorized;

        // Act
        var actual = Result.Unauthorized().ToActionResult();

        // Assert
        actual.Should().NotBeNull().And.BeOfType<UnauthorizedObjectResult>();
        var apiResponse = actual.ToApiResponse();
        apiResponse.Should().NotBeNull();
        apiResponse.Success.Should().BeFalse();
        apiResponse.StatusCode.Should().Be(expectedStatusCode);
    }

    [Fact]
    public void Should_ReturnsForbiddenObjectResult_WhenResultHasNotFoundError()
    {
        // Arrange
        const int expectedStatusCode = StatusCodes.Status403Forbidden;

        // Act
        var actual = Result.Forbidden().ToActionResult();

        // Assert
        actual.Should().NotBeNull().And.BeOfType<ForbiddenObjectResult>();
        var apiResponse = actual.ToApiResponse();
        apiResponse.Should().NotBeNull();
        apiResponse.Success.Should().BeFalse();
        apiResponse.StatusCode.Should().Be(expectedStatusCode);
    }

    [Fact]
    public void Should_ReturnsOkResult_WhenResultIsOk()
    {
        // Arrange
        const int expectedStatusCode = StatusCodes.Status200OK;

        // Act
        var actual = Result.Success().ToActionResult();

        // Assert
        actual.Should().NotBeNull().And.BeOfType<OkObjectResult>();
        var apiResponse = actual.ToApiResponse();
        apiResponse.Should().NotBeNull();
        apiResponse.Success.Should().BeTrue();
        apiResponse.StatusCode.Should().Be(expectedStatusCode);
        apiResponse.Errors.Should().BeNullOrEmpty();
    }

    [Fact]
    public void Should_ReturnsOkResult_WhenTypedResultIsOk()
    {
        // Arrange
        const string resultValue = "Hello World!!!";
        const int expectedStatusCode = StatusCodes.Status200OK;

        // Act
        var actual = Result<string>.Success(resultValue).ToActionResult();

        // Assert
        actual.Should().NotBeNull().And.BeOfType<OkObjectResult>();
        var apiResponse = actual.ToApiResponse<string>();
        apiResponse.Should().NotBeNull();
        apiResponse.Success.Should().BeTrue();
        apiResponse.StatusCode.Should().Be(expectedStatusCode);
        apiResponse.Errors.Should().BeNullOrEmpty();
        apiResponse.Result.Should().NotBeNullOrWhiteSpace().And.Be(resultValue);
    }
}