using System;
using FluentAssertions;
using SGP.Application.Requests;
using SGP.Shared.Helpers;
using SGP.Tests.Constants;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Shared.Helpers
{
    [Category(TestCategories.Shared)]
    public class ValidatorHelperTests
    {
        [Fact]
        public void Should_ReturnsValidationResult_WhenValidate()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var request = new GetByIdRequest(id);

            // Act
            var act = ValidatorHelper.Validate<GetByIdRequestValidator>(request);

            // Assert
            act.Should().NotBeNull();
            act.IsValid.Should().BeTrue();
            act.Errors.Should().BeNullOrEmpty();
        }

        [Fact]
        public void Should_ReturnsValidationResult_WhenValidateWithValidatorCached()
        {
            // Arrange
            // Primeiro uso, criando o cache da instancia do validador
            ValidatorHelper.Validate<GetByIdRequestValidator>(new GetByIdRequest(Guid.NewGuid()));
            var request = new GetByIdRequest(string.Empty);

            // Act
            var act = ValidatorHelper.Validate<GetByIdRequestValidator>(request);

            // Assert
            act.Should().NotBeNull();
            act.IsValid.Should().BeFalse();
            act.Errors.Should().NotBeNullOrEmpty().And.OnlyHaveUniqueItems();
        }
    }
}