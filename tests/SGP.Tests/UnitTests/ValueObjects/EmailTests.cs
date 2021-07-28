using Xunit;
using SGP.Domain.ValueObjects;
using FluentAssertions;
using FluentValidation;
using System;

namespace SGP.Tests.UnitTests.ValueObjects
{
    public class EmailTests
    {
        [Theory]
        [ClassData(typeof(TestDatas.ValidEmailAddresses))]
        public void Devera_RetornarInstancia_QuandoEnderecoEmailForValido(string address)
        {
            // Act
            var act = new Email(address);

            // Assert
            act.Should().NotBeNull();
            act.Address.Should().NotBeNullOrWhiteSpace().And.Be(address.ToLowerInvariant());
        }

        [Theory]
        [ClassData(typeof(TestDatas.InvalidEmailAddresses))]
        public void Devera_Lancar_QuandoEnderecoEmailForInvalido(string address)
        {
            // Act
            Action act = () => new Email(address);

            // Assert
            act.Should().ThrowExactly<ValidationException>();
        }
    }
}