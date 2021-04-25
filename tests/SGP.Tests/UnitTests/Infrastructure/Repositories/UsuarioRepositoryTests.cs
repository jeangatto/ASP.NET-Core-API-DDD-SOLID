using FluentAssertions;
using SGP.Domain.Entities.UsuarioAggregate;
using SGP.Domain.Repositories;
using SGP.Domain.ValueObjects;
using SGP.Infrastructure.Repositories;
using SGP.Tests.Fixtures;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Infrastructure.Repositories
{
    [Category(TestCategories.Infrastructure)]
    public class UsuarioRepositoryTests : IClassFixture<EfFixture>
    {
        private readonly EfFixture _fixture;

        public UsuarioRepositoryTests(EfFixture fixture)
        {
            _fixture = fixture;
            Task.Run(() => SeedAsync()).Wait();
        }

        [Fact]
        [UnitTest]
        public async Task Should_ReturnsUser_WhenEmailExists()
        {
            // Arrange
            var repository = CreateRepository();
            var email = Email.Create("john_doe@hotmail.com").Value;

            // Act
            var act = await repository.GetByEmailAsync(email);

            // Assert
            act.Should().NotBeNull().And.Match<Usuario>((u)
                => u.Nome == "John Doe" && u.Senha == "a1b2c3d4");
        }

        private IUsuarioRepository CreateRepository()
        {
            return new UsuarioRepository(_fixture.Context);
        }

        private async Task SeedAsync()
        {
            _fixture.Context.AddRange(new[]
            {
                new Usuario("John Doe", Email.Create("john_doe@hotmail.com").Value, "a1b2c3d4"),
                new Usuario("Mary Doe", Email.Create("mary.doe@gmail.com").Value, "4d3c2b1a"),
                new Usuario("Alan Doe", Email.Create("alan.doe@outlook.com").Value, "a1d4c3b2")
            });

            await _fixture.Context.SaveChangesAsync();
        }
    }
}