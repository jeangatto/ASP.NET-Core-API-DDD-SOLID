using Ardalis.Result;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.PublicApi.Extensions;
using SGP.PublicApi.Models;
using SGP.Tests.Extensions;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.PublicApi.Extensions;

[UnitTest]
public class FluentResultExtensionsTests
{
    [Fact]
    public void Should_ReturnsBadRequestResult_WhenTypedResultHasError()
    {
        // Act
        const int expectedStatusCode = StatusCodes.Status400BadRequest;
        const string errorMessage = "Requisição inválida.";

        // Arrange
        var actual = Result.Error(errorMessage).ToActionResult();

        // Assert
        actual.Should().NotBeNull().And.BeOfType<BadRequestObjectResult>();

        var objectResult = actual as BadRequestObjectResult;
        objectResult.Should().NotBeNull();

        var apiResponse = objectResult.Value as ApiResponse;
        apiResponse.Should().NotBeNull();
        apiResponse.Success.Should().BeFalse();
        apiResponse.StatusCode.Should().Be(expectedStatusCode);
        apiResponse.Errors.Should().NotBeNullOrEmpty()
            .And.OnlyHaveUniqueItems()
            .And.ContainSingle()
            .And.Subject.ForEach(error => error.Message.Should().NotBeNullOrWhiteSpace().And.Be(errorMessage));
    }

    [Fact]
    public void Should_ReturnsDistinctErrorsMessage_WhenResultHasErrors()
    {
        // Arrange
        const int expectedStatusCode = StatusCodes.Status400BadRequest;
        var errors = new[] { "Erro0", "Erro1", "Erro3", "Erro2" };

        // Act
        var actual = Result.Error(errors).ToActionResult();

        // Assert
        actual.Should().NotBeNull().And.BeOfType<BadRequestObjectResult>();

        var objectResult = actual as BadRequestObjectResult;
        objectResult.Should().NotBeNull();

        var apiResponse = objectResult.Value as ApiResponse;
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
        // Act
        const int expectedStatusCode = StatusCodes.Status404NotFound;
        const string errorMessage = "Nenhum registro encontrado.";

        // Arrange
        var actual = Result.NotFound(errorMessage).ToActionResult();

        // Assert
        actual.Should().NotBeNull().And.BeOfType<NotFoundObjectResult>();

        var objectResult = actual as NotFoundObjectResult;
        objectResult.Should().NotBeNull();

        var apiResponse = objectResult.Value as ApiResponse;
        apiResponse.Should().NotBeNull();
        apiResponse.Success.Should().BeFalse();
        apiResponse.StatusCode.Should().Be(expectedStatusCode);
        apiResponse.Errors.Should().NotBeNullOrEmpty()
            .And.OnlyHaveUniqueItems()
            .And.ContainSingle()
            .And.Subject.ForEach(error => error.Message.Should().NotBeNullOrWhiteSpace().And.Be(errorMessage));
    }

    [Fact]
    public void Should_ReturnsOkResult_WhenResultIsOk()
    {
        // Act
        const int expectedStatusCode = StatusCodes.Status200OK;

        // Arrange
        var actual = Result.Success().ToActionResult();

        // Assert
        actual.Should().NotBeNull().And.BeOfType<OkObjectResult>();

        var objectResult = actual as OkObjectResult;
        objectResult.Should().NotBeNull();

        var apiResponse = objectResult.Value as ApiResponse;
        apiResponse.Should().NotBeNull();
        apiResponse.Success.Should().BeTrue();
        apiResponse.StatusCode.Should().Be(expectedStatusCode);
        apiResponse.Errors.Should().BeNullOrEmpty();
    }

    [Fact]
    public void Should_ReturnsOkResult_WhenTypedResultIsOk()
    {
        // Act
        const string resultValue = "Hello World!!!";
        const int expectedStatusCode = StatusCodes.Status200OK;

        // Arrange
        var actual = Result<string>.Success(resultValue).ToActionResult();

        // Assert
        actual.Should().NotBeNull().And.BeOfType<OkObjectResult>();

        var objectResult = actual as OkObjectResult;
        objectResult.Should().NotBeNull();

        var apiResponse = objectResult.Value as ApiResponse<string>;
        apiResponse.Should().NotBeNull();
        apiResponse.Success.Should().BeTrue();
        apiResponse.StatusCode.Should().Be(expectedStatusCode);
        apiResponse.Errors.Should().BeNullOrEmpty();
        apiResponse.Result.Should().NotBeNullOrWhiteSpace().And.Be(resultValue);
    }
}