using FluentAssertions;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.PublicApi.Extensions;
using SGP.PublicApi.Models;
using SGP.Shared.Errors;
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
        // Act
        const int expectedStatusCode = StatusCodes.Status400BadRequest;
        const string errorMessage = "Requisição inválida.";
        var expectedApiResponse = new ApiResponse(false, expectedStatusCode, new ApiError(errorMessage));
        var result = new Result().WithError(new Error(errorMessage));

        // Arrange
        var actual = result.ToHttpResult();

        // Assert
        actual.Should().NotBeNull().And.BeOfType<BadRequestObjectResult>();
        actual.StatusCode.Should().Be(expectedStatusCode);
        actual.Value.Should().BeEquivalentTo(expectedApiResponse);
    }

    [Fact]
    public void Should_ReturnsBadRequestResult_WhenTypedResultHasError()
    {
        // Act
        const int expectedStatusCode = StatusCodes.Status400BadRequest;
        const string errorMessage = "Requisição inválida.";
        var expectedApiResponse = new ApiResponse(false, expectedStatusCode, new ApiError(errorMessage));
        var result = new Result<string>().WithError(new Error(errorMessage));

        // Arrange
        var actual = result.ToHttpResult();

        // Assert
        actual.Should().NotBeNull().And.BeOfType<BadRequestObjectResult>();
        actual.StatusCode.Should().Be(expectedStatusCode);
        actual.Value.Should().BeEquivalentTo(expectedApiResponse);
    }

    [Fact]
    public void Should_ReturnsDistinctErrorsMessage_WhenResultHasErrors()
    {
        // Arrange
        const int expectedCount = 3;
        var result = new Result().WithErrors(new[] { "Erro0", "Erro1", "Erro0", "Erro2" });

        // Act
        var actual = result.ToHttpResult();

        // Assert
        actual.Should().NotBeNull().And.BeOfType<BadRequestObjectResult>();
        actual.Value.Should().NotBeNull().And.BeOfType<ApiResponse>()
            .Subject.Errors.Should().NotBeNullOrEmpty()
            .And.OnlyHaveUniqueItems()
            .And.HaveCount(expectedCount)
            .And.Subject.ForEach(error => error.Message.Should().NotBeNullOrWhiteSpace());
    }

    [Fact]
    public void Should_ReturnsNotFoundRequestResult_WhenResultHasNotFoundError()
    {
        // Act
        const int expectedStatusCode = StatusCodes.Status404NotFound;
        const string errorMessage = "Nenhum registro encontrado.";
        var expectedApiResponse = new ApiResponse(false, expectedStatusCode, new ApiError(errorMessage));
        var result = new Result().WithError(new NotFoundError(errorMessage));

        // Arrange
        var actual = result.ToHttpResult();

        // Assert
        actual.Should().NotBeNull().And.BeOfType<NotFoundObjectResult>();
        actual.StatusCode.Should().Be(expectedStatusCode);
        actual.Value.Should().BeEquivalentTo(expectedApiResponse);
    }

    [Fact]
    public void Should_ReturnsNotFoundRequestResult_WhenTypedResultHasNotFoundError()
    {
        // Act
        const int expectedStatusCode = StatusCodes.Status404NotFound;
        const string errorMessage = "Nenhum registro encontrado.";
        var expectedApiResponse = new ApiResponse(false, expectedStatusCode, new ApiError(errorMessage));
        var result = new Result<string>().WithError(new NotFoundError(errorMessage));

        // Arrange
        var actual = result.ToHttpResult();

        // Assert
        actual.Should().NotBeNull().And.BeOfType<NotFoundObjectResult>();
        actual.StatusCode.Should().Be(expectedStatusCode);
        actual.Value.Should().BeEquivalentTo(expectedApiResponse);
    }

    [Fact]
    public void Should_ReturnsOkResult_WhenResultIsOk()
    {
        // Act
        const int expectedStatusCode = StatusCodes.Status200OK;
        var expectedApiResponse = new ApiResponse(true, expectedStatusCode);
        var result = new Result();

        // Arrange
        var actual = result.ToHttpResult();

        // Assert
        actual.Should().NotBeNull().And.BeOfType<OkObjectResult>();
        actual.StatusCode.Should().Be(expectedStatusCode);
        actual.Value.Should().BeEquivalentTo(expectedApiResponse);
    }

    [Fact]
    public void Should_ReturnsOkResult_WhenTypedResultIsOk()
    {
        // Act
        const string value = "Hello World!!!";
        const int expectedStatusCode = StatusCodes.Status200OK;
        var expectedApiResponse = new ApiResponse<string>(true, expectedStatusCode, value);
        var result = new Result<string>().WithValue(value);

        // Arrange
        var actual = result.ToHttpResult();

        // Assert
        actual.Should().NotBeNull().And.BeOfType<OkObjectResult>();
        actual.StatusCode.Should().Be(expectedStatusCode);
        actual.Value.Should().BeEquivalentTo(expectedApiResponse);
    }
}