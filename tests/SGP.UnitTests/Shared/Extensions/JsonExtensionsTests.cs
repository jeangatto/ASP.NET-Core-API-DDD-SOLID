using FluentAssertions;
using SGP.Shared.Extensions;
using SGP.SharedTests.Constants;
using Xunit;
using Xunit.Categories;

namespace SGP.UnitTests.Shared.Extensions
{
    [UnitTest(TestCategories.Shared)]
    public class JsonExtensionsTests
    {
        [Fact]
        public void Should_ReturnJsonString_WhenSerializeWithPrivateSetters()
        {
            // Arrange
            const string expectedJson = "{\"email\":\"john.doe@hotmai.com\",\"userName\":\"John Doe\",\"status\":\"active\"}";
            var user = new User("John Doe", "john.doe@hotmai.com", Status.Active);

            // Act
            var actual = user.ToJson();

            // Assert
            actual.Should().NotBeNullOrEmpty().And.BeEquivalentTo(expectedJson);
        }

        [Fact]
        public void Should_ReturnEntity_WhenDeserializeWithPrivateSetters()
        {
            // Arrange
            var expectedUser = new User("John Doe", "john.doe@hotmai.com", Status.Inactive);
            const string json = "{\"email\":\"john.doe@hotmai.com\",\"userName\":\"John Doe\",\"status\":\"inactive\"}";

            // Act
            var actual = json.FromJson<User>();

            // Assert
            actual.Should().NotBeNull().And.BeEquivalentTo(expectedUser);
        }

        private enum Status { Active = 0, Inactive = 1 }

        private class User
        {
            public User(string userName, string email, Status status)
            {
                UserName = userName;
                Email = email;
                Status = status;
            }

            public string Email { get; private set; }
            public string UserName { get; private set; }
            public Status Status { get; private set; }
        }
    }
}