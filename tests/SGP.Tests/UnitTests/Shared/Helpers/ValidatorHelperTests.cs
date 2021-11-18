using System;
using FluentAssertions;
using SGP.Application.Requests;
using SGP.Shared.Helpers;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Shared.Helpers
{
    [UnitTest]
    public class ValidatorHelperTests
    {
        [Fact]
        public void Should_ReturnsValidationResult_WhenValidate()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var request = new GetByIdRequest(id);

            // Act
            var actual = ValidatorHelper.Validate<GetByIdRequestValidator>(request);

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
            ValidatorHelper.Validate<GetByIdRequestValidator>(new GetByIdRequest(Guid.NewGuid()));
            var request = new GetByIdRequest(string.Empty);

            // Act
            var actual = ValidatorHelper.Validate<GetByIdRequestValidator>(request);

            // Assert
            actual.Should().NotBeNull();
            actual.IsValid.Should().BeFalse();
            actual.Errors.Should().NotBeNullOrEmpty().And.OnlyHaveUniqueItems();
        }
    }
}