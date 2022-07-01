using FluentAssertions;
using SGP.Shared.Extensions;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Shared.Extensions;

[UnitTest]
public class JsonExtensionsTests
{
    private const string UserJson = "{\"email\":\"john.doe@hotmail.com\",\"userName\":\"John Doe\",\"status\":\"active\"}";

    [Fact]
    public void Should_ReturnJsonString_WhenSerialize()
    {
        // Arrange
        var user = new User("John Doe", "john.doe@hotmail.com", EStatus.Active);

        // Act
        var act = user.ToJson();

        // Assert
        act.Should().NotBeNullOrWhiteSpace().And.BeEquivalentTo(UserJson);
    }

    [Fact]
    public void Should_ReturnEntity_WhenDeserialize()
    {
        // Arrange
        var expectedUser = new User("John Doe", "john.doe@hotmail.com", EStatus.Active);

        // Act
        var act = UserJson.FromJson(typeof(User));

        // Assert
        act.Should().NotBeNull().And.BeOfType<User>().And.BeEquivalentTo(expectedUser);
    }

    [Fact]
    public void Should_ReturnEntity_WhenDeserializeTyped()
    {
        // Arrange
        var expectedUser = new User("John Doe", "john.doe@hotmail.com", EStatus.Active);

        // Act
        var act = UserJson.FromJson<User>();

        // Assert
        act.Should().NotBeNull().And.BeEquivalentTo(expectedUser);
    }

    [Fact]
    public void Should_ReturnNull_WhenSerializeNullValue()
    {
        // Arrange
        User user = null;

        // Act
        var act = user.ToJson();

        // Assert
        act.Should().BeNull();
    }

    [Fact]
    public void Should_ReturnNull_WhenDeserializeNullValueTyped()
    {
        // Arrange
        const string strJson = null;

        // Act
        var act = strJson.FromJson<User>();

        // Assert
        act.Should().BeNull();
    }

    [Fact]
    public void Should_ReturnNull_WhenDeserializeNullValue()
    {
        // Arrange
        const string strJson = null;

        // Act
        var act = strJson.FromJson(typeof(User));

        // Assert
        act.Should().BeNull();
    }

    private enum EStatus
    {
        Active = 0,
        Inactive = 1
    }

    private record User
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