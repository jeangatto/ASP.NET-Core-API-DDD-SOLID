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
    public void Should_ReturnsBadRequestResult_WhenResultHasBusinessError()
    {
        // Act
        const int expectedStatusCode = StatusCodes.Status400BadRequest;
        const string errorMessage = "Requisição inválida.";
        var expectedApiResponse = ApiResponse.BadRequest(errorMessage);
        var result = new Result().WithError(new BusinessError(errorMessage));

        // Arrange
        var actual = result.ToHttpResult();

        // Assert
        actual.Should().NotBeNull().And.BeOfType<BadRequestObjectResult>();
        actual.StatusCode.Should().Be(expectedStatusCode);
        actual.Value.Should().BeEquivalentTo(expectedApiResponse);
        var apiResponse = actual.Value.As<ApiResponse>();
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
        // Act
        const int expectedStatusCode = StatusCodes.Status400BadRequest;
        const string errorMessage = "Requisição inválida.";
        var expectedApiResponse = ApiResponse.BadRequest(errorMessage);
        var result = new Result<string>().WithError(new ValidationError(errorMessage));

        // Arrange
        var actual = result.ToHttpResult();

        // Assert
        actual.Should().NotBeNull().And.BeOfType<BadRequestObjectResult>();
        actual.StatusCode.Should().Be(expectedStatusCode);
        actual.Value.Should().BeEquivalentTo(expectedApiResponse);
        var apiResponse = actual.Value.As<ApiResponse>();
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
        const int expectedStatusCode = StatusCodes.Status500InternalServerError;
        var result = new Result().WithErrors(new[] {"Erro0", "Erro1", "Erro3", "Erro2"});

        // Act
        var actual = result.ToHttpResult();

        // Assert
        actual.Should().NotBeNull();
        actual.Value.Should().NotBeNull().And.BeOfType<ApiResponse>();
        var apiResponse = actual.Value.As<ApiResponse>();
        apiResponse.Success.Should().BeFalse();
        apiResponse.StatusCode.Should().Be(expectedStatusCode);
        apiResponse.Errors.Should().NotBeNullOrEmpty()
            .And.OnlyHaveUniqueItems()
            .And.HaveCount(result.Errors.Count)
            .And.Subject.ForEach(error => error.Message.Should().NotBeNullOrWhiteSpace());
    }

    [Fact]
    public void Should_ReturnsNotFoundRequestResult_WhenResultHasNotFoundError()
    {
        // Act
        const int expectedStatusCode = StatusCodes.Status404NotFound;
        const string errorMessage = "Nenhum registro encontrado.";
        var expectedApiResponse = ApiResponse.NotFound(errorMessage);
        var result = new Result().WithError(new NotFoundError(errorMessage));

        // Arrange
        var actual = result.ToHttpResult();

        // Assert
        actual.Should().NotBeNull().And.BeOfType<NotFoundObjectResult>();
        actual.StatusCode.Should().Be(expectedStatusCode);
        actual.Value.Should().BeEquivalentTo(expectedApiResponse);
        var apiResponse = actual.Value.As<ApiResponse>();
        apiResponse.Success.Should().BeFalse();
        apiResponse.StatusCode.Should().Be(expectedStatusCode);
        apiResponse.Errors.Should().NotBeNullOrEmpty()
            .And.OnlyHaveUniqueItems()
            .And.ContainSingle()
            .And.Subject.ForEach(error => error.Message.Should().NotBeNullOrWhiteSpace().And.Be(errorMessage));
    }

    [Fact]
    public void Should_ReturnsNotFoundRequestResult_WhenTypedResultHasNotFoundError()
    {
        // Act
        const int expectedStatusCode = StatusCodes.Status404NotFound;
        const string errorMessage = "Nenhum registro encontrado.";
        var expectedApiResponse = ApiResponse.NotFound(errorMessage);
        var result = new Result<string>().WithError(new NotFoundError(errorMessage));

        // Arrange
        var actual = result.ToHttpResult();

        // Assert
        actual.Should().NotBeNull().And.BeOfType<NotFoundObjectResult>();
        actual.StatusCode.Should().Be(expectedStatusCode);
        actual.Value.Should().BeEquivalentTo(expectedApiResponse);
        var apiResponse = actual.Value.As<ApiResponse>();
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
        var expectedApiResponse = ApiResponse.Ok();
        var result = new Result();

        // Arrange
        var actual = result.ToHttpResult();

        // Assert
        actual.Should().NotBeNull().And.BeOfType<OkObjectResult>();
        actual.StatusCode.Should().Be(expectedStatusCode);
        actual.Value.Should().BeEquivalentTo(expectedApiResponse);
        var apiResponse = actual.Value.As<ApiResponse>();
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
        var expectedApiResponse = ApiResponse<string>.Ok(resultValue);
        var result = new Result<string>().WithValue(resultValue);

        // Arrange
        var actual = result.ToHttpResult();

        // Assert
        actual.Should().NotBeNull().And.BeOfType<OkObjectResult>();
        actual.StatusCode.Should().Be(expectedStatusCode);
        actual.Value.Should().BeEquivalentTo(expectedApiResponse);
        var apiResponse = actual.Value.As<ApiResponse<string>>();
        apiResponse.Success.Should().BeTrue();
        apiResponse.StatusCode.Should().Be(expectedStatusCode);
        apiResponse.Result.Should().NotBeNullOrWhiteSpace().And.Be(resultValue);
        apiResponse.Errors.Should().BeNullOrEmpty();
    }
}