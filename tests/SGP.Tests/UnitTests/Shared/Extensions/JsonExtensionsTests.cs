using FluentAssertions;
using SGP.Shared.Extensions;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Shared.Extensions
{
    [UnitTest]
    public class JsonExtensionsTests
    {
        [Fact]
        public void Should_ReturnJsonString_WhenSerialize()
        {
            // Arrange
            const string expectedJson = "{\"email\":\"john.doe@hotmai.com\",\"userName\":\"John Doe\",\"status\":\"active\"}";
            var user = new User("John Doe", "john.doe@hotmai.com", EStatus.Active);

            // Act
            var actual = user.ToJson();

            // Assert
            actual.Should().NotBeNullOrEmpty().And.BeEquivalentTo(expectedJson);
        }

        [Fact]
        public void Should_ReturnEntity_WhenDeserialize()
        {
            // Arrange
            var expectedUser = new User("John Doe", "john.doe@hotmai.com", EStatus.Inactive);
            const string json = "{\"email\":\"john.doe@hotmai.com\",\"userName\":\"John Doe\",\"status\":\"inactive\"}";

            // Act
            var actual = json.FromJson(typeof(User));

            // Assert
            actual.Should().NotBeNull().And.BeOfType<User>().And.BeEquivalentTo(expectedUser);
        }

        [Fact]
        public void Should_ReturnEntity_WhenDeserializeTyped()
        {
            // Arrange
            var expectedUser = new User("John Doe", "john.doe@hotmai.com", EStatus.Inactive);
            const string json = "{\"email\":\"john.doe@hotmai.com\",\"userName\":\"John Doe\",\"status\":\"inactive\"}";

            // Act
            var actual = json.FromJson<User>();

            // Assert
            actual.Should().NotBeNull().And.BeEquivalentTo(expectedUser);
        }

        private enum EStatus { Active = 0, Inactive = 1 }

        private class User
        {
            public User(string userName, string email, EStatus status)
            {
                UserName = userName;
                Email = email;
                Status = status;
            }

            public string Email { get; private set; }
            public string UserName { get; private set; }
            public EStatus Status { get; private set; }
        }
    }
}