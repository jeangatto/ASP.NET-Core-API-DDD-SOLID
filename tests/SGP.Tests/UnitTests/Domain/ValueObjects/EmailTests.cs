using FluentAssertions;
using FluentResults.Extensions.FluentAssertions;
using SGP.Domain.ValueObjects;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Domain.ValueObjects
{
    [Category(TestCategories.Domain)]
    public class EmailTests
    {
        [Theory]
        [UnitTest]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("Abc.example.com")]     // No `@`
        [InlineData("A@b@c@example.com")]   // multiple `@`
        [InlineData("ma...ma@jjf.co")]      // continuous multiple dots in name
        [InlineData("ma@jjf.c")]            // only 1 char in extension
        [InlineData("ma@jjf..com")]         // continuous multiple dots in domain
        [InlineData("ma@@jjf.com")]         // continuous multiple `@`
        [InlineData("@majjf.com")]          // nothing before `@`
        [InlineData("ma.@jjf.com")]         // nothing after `.`
        [InlineData("ma_@jjf.com")]         // nothing after `_`
        [InlineData("ma_@jjf")]             // no domain extension
        [InlineData("ma_@jjf.")]            // nothing after `_` and .
        [InlineData("ma@jjf.")]             // nothing after `.`
        public void Should_WhenEmailIsInvalid_ReturnsError(string address)
        {
            // Act
            var act = Email.Create(address);

            // Assert
            act.Should().NotBeNull()
                .And.BeFailure()
                .And.Satisfy(r => r.Errors.Should().NotBeEmpty().And.OnlyHaveUniqueItems());
        }

        [Theory]
        [UnitTest]
        [InlineData("ma@hostname.com")]
        [InlineData("ma@hostname.comcom")]
        [InlineData("MA@hostname.coMCom")]
        [InlineData("MA@HOSTNAME.COM")]
        [InlineData("m.a@hostname.co")]
        [InlineData("m_a1a@hostname.com")]
        [InlineData("ma-a@hostname.com")]
        [InlineData("ma-a@hostname.com.edu")]
        [InlineData("ma-a.aa@hostname.com.edu")]
        [InlineData("ma.h.saraf.onemore@hostname.com.edu")]
        [InlineData("ma12@hostname.com")]
        [InlineData("12@hostname.com")]
        public void Should_ReturnsSuccess_WhenEmailIsValid(string address)
        {
            // Act
            var act = Email.Create(address);

            // Assert
            act.Should().NotBeNull().And.BeSuccess();
            act.Value.Address.Should().Be(address.ToLowerInvariant());
        }
    }
}