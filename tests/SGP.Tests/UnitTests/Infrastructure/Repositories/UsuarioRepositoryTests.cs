using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SGP.Domain.Entities.UsuarioAggregate;
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
    public class UsuarioRepositoryTests : IClassFixture<EfSqliteFixture>
    {
        private readonly EfSqliteFixture _fixture;

        public UsuarioRepositoryTests(EfSqliteFixture fixture)
        {
            _fixture = fixture;
            SeedUsers(_fixture.Context);
        }

        [Fact]
        [UnitTest]
        public async Task Should_ReturnsUser_WhenEmailExists()
        {
            // Arrange
            var repository = new UsuarioRepository(_fixture.Context);
            var email = new Email("john_doe@hotmail.com");

            // Act
            var act = await repository.GetByEmailAsync(email);

            // Assert
            act.Should().NotBeNull()
                .And.Match<Usuario>((u) => u.Nome == "John Doe" && u.Senha == "a1b2c3d4");
        }

        private static void SeedUsers(SgpContext context)
        {
            if (!context.Usuarios.AsNoTracking().Any())
            {
                context.Usuarios.AddRange(new[]
                {
                    new Usuario("John Doe", new Email("john_doe@hotmail.com"), "a1b2c3d4"),
                    new Usuario("Mary Doe", new Email("mary.doe@gmail.com"), "4d3c2b1a"),
                    new Usuario("Alan Doe", new Email("alan.doe@outlook.com"), "a1d4c3b2")
                });
                context.SaveChanges();
            }
        }
    }
}