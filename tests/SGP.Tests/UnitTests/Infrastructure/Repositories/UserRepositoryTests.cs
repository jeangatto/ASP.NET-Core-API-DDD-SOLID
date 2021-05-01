using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SGP.Domain.Entities.UserAggregate;
using SGP.Domain.ValueObjects;
using SGP.Infrastructure.Context;
using SGP.Infrastructure.Repositories;
using SGP.Tests.Fixtures;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Infrastructure.Repositories
{
    [Category(TestCategories.Infrastructure)]
    public class UserRepositoryTests : IClassFixture<EfSqliteFixture>
    {
        private readonly EfSqliteFixture _fixture;

        public UserRepositoryTests(EfSqliteFixture fixture)
        {
            _fixture = fixture;
            SeedUsers(_fixture.Context);
        }

        [Fact]
        [UnitTest]
        public async Task Should_ReturnsUser_WhenEmailExists()
        {
            // Arrange
            var repository = new UserRepository(_fixture.Context);
            var email = new Email("john_doe@hotmail.com");

            // Act
            var act = await repository.GetByEmailAsync(email);

            // Assert
            act.Should().NotBeNull()
                .And.Match<User>((u) => u.Name == "John Doe" && u.PasswordHash == "a1b2c3d4");
        }

        private static void SeedUsers(SgpContext context)
        {
            if (!context.Users.AsNoTracking().Any())
            {
                context.Users.AddRange(new[]
                {
                    new User("John Doe", new Email("john_doe@hotmail.com"), "a1b2c3d4"),
                    new User("Mary Doe", new Email("mary.doe@gmail.com"), "4d3c2b1a"),
                    new User("Alan Doe", new Email("alan.doe@outlook.com"), "a1d4c3b2")
                });
                context.SaveChanges();
            }
        }
    }
}