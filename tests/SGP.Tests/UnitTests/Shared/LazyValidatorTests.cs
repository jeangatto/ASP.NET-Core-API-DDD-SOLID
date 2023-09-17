using System;
using System.Threading.Tasks;
using FluentAssertions;
using SGP.Application.Requests;
using SGP.Shared;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Shared;

[UnitTest]
public class LazyValidatorTests
{
    [Fact]
    public void Should_ReturnsValidationResult_WhenValidate()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var request = new GetByIdRequest(id);

        // Act
        var actual = LazyValidator.Validate<GetByIdRequestValidator>(request);

        // Assert
        actual.Should().NotBeNull();
        actual.IsValid.Should().BeTrue();
        actual.Errors.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task Should_ReturnsValidationResult_WhenValidateAsync()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var request = new GetByIdRequest(id);

        // Act
        var actual = await LazyValidator.ValidateAsync<GetByIdRequestValidator>(request);

        // Assert
        actual.Should().NotBeNull();
        actual.IsValid.Should().BeTrue();
        actual.Errors.Should().BeNullOrEmpty();
    }

    [Fact]
    public void Should_ReturnsValidationResult_WhenValidateWithValidatorCached()
    {
        // Arrange
        // Primeiro uso, criando o cache da instancia do validador
        LazyValidator.Validate<GetByIdRequestValidator>(new GetByIdRequest(Guid.NewGuid()));
        var request = new GetByIdRequest(string.Empty);

        // Act
        var actual = LazyValidator.Validate<GetByIdRequestValidator>(request);

        // Assert
        actual.Should().NotBeNull();
        actual.IsValid.Should().BeFalse();
        actual.Errors.Should().NotBeNullOrEmpty().And.OnlyHaveUniqueItems();
    }
}
