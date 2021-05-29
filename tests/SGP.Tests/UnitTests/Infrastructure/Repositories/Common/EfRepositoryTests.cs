using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Domain.ValueObjects;
using SGP.Infrastructure.Repositories;
using SGP.Infrastructure.UoW;
using SGP.Shared.Interfaces;
using SGP.Tests.Extensions;
using SGP.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Infrastructure.Repositories.Common
{
    [Category(TestCategories.Infrastructure)]
    public class EfRepositoryTests : UnitTestBase, IClassFixture<EfSqliteFixture>
    {
        private readonly EfSqliteFixture _fixture;

        public EfRepositoryTests(EfSqliteFixture fixture) => _fixture = fixture;

        private static Faker Faker => new();

        [Fact]
        public async Task Should_AddEntity()
        {
            // Arrange
            var uow = CriarUoW();
            var repositorio = CriarRepositorio();

            // Act
            repositorio.Add(CriarUsuarioFaker().Generate());
            var actual = await uow.SaveChangesAsync();

            // Assert
            actual.Should().Be(1);
        }

        [Fact]
        public async Task Should_AddRangeEntities()
        {
            // Arrange
            const int quantidade = 3;
            var uow = CriarUoW();
            var repositorio = CriarRepositorio();

            // Act
            repositorio.AddRange(CriarUsuarioFaker().Generate(quantidade));
            var actual = await uow.SaveChangesAsync();

            // Assert
            actual.Should().Be(quantidade);
        }

        [Fact]
        public async Task Should_RemoveEntity()
        {
            // Arrange
            const int quantidade = 1;
            var (repositorio, uow, usuarios) = await InserirUsuarios(quantidade);
            var id = usuarios.First().Id;

            // Act
            repositorio.Remove(usuarios.First());
            var actual = await uow.SaveChangesAsync();
            var removido = await repositorio.GetByIdAsync(id);

            // Assert
            actual.Should().Be(quantidade);
            removido.Should().BeNull();
        }

        [Fact]
        public async Task Should_RemoveRangeEntities()
        {
            // Arrange
            const int quantidade = 4;
            var (repositorio, uow, usuarios) = await InserirUsuarios(quantidade);
            var ids = usuarios.Select(u => u.Id).ToList();

            // Act
            repositorio.RemoveRange(usuarios);
            var actual = await uow.SaveChangesAsync();
            var removidos = (from id in ids select repositorio.GetByIdAsync(id).GetAwaiter().GetResult())
                .Where(usuario => usuario != null).ToList();

            // Assert
            actual.Should().Be(quantidade);
            removidos.Should().BeEmpty();
        }

        [Fact]
        public async Task Should_ReturnNonTrackedEntity_WhenGetById()
        {
            // Arrange
            var (repositorio, _, usuarios) = await InserirUsuarios(1);

            // Act
            var actual = await repositorio.GetByIdAsync(usuarios.First().Id, readOnly: true);
            var isTracked = _fixture.Context.Usuarios.Local.Any(usuario => usuario == actual);

            // Assert
            isTracked.Should().BeFalse();
            actual.Should().NotBeNull();
            actual.Id.Should().NotBeEmpty();
            actual.Nome.Should().NotBeNullOrWhiteSpace();
            actual.Email.Should().NotBeNull();
            actual.HashSenha.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Should_ReturnTrackedEntity_WhenGetById()
        {
            // Arrange
            var (repositorio, _, usuarios) = await InserirUsuarios(1);

            // Act
            var actual = await repositorio.GetByIdAsync(usuarios.First().Id, readOnly: false);
            var isTracked = _fixture.Context.Usuarios.Local.Any(usuario => usuario == actual);

            // Assert
            isTracked.Should().BeTrue();
            actual.Should().NotBeNull();
            actual.Id.Should().NotBeEmpty();
            actual.Nome.Should().NotBeNullOrWhiteSpace();
            actual.Email.Should().NotBeNull();
            actual.HashSenha.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Should_UpdateEntity()
        {
            // Arrange
            var (repositorio, uow, usuarios) = await InserirUsuarios(1);
            var usuario = usuarios.First();
            usuario.AtualizarNome(Faker.Person.UserName);

            // Act
            repositorio.Update(usuario);
            var actual = await uow.SaveChangesAsync();
            var atualizado = await repositorio.GetByIdAsync(usuario.Id);

            // Assert
            actual.Should().BePositive();
            atualizado.Should().NotBeNull();
            atualizado.Id.Should().Be(usuario.Id);
            atualizado.Nome.Should().Be(usuario.Nome);
        }

        [Fact]
        public async Task Should_UpdateRangeEntities()
        {
            // Arrange
            const int quantidade = 2;
            var (repositorio, uow, usuarios) = await InserirUsuarios(quantidade);
            usuarios.ForEach(u => u.AtualizarNome(Faker.Person.UserName));
            var ids = usuarios.Select(u => u.Id).ToList();

            // Act
            repositorio.UpdateRange(usuarios);
            var actual = await uow.SaveChangesAsync();
            var atualizados = (from id in ids select repositorio.GetByIdAsync(id).GetAwaiter().GetResult()).ToList();

            // Assert
            actual.Should().Be(quantidade);
            atualizados.Should().NotBeEmpty().And.Subject.ForEach(u
                => u.Nome.Should().Be(usuarios.First(x => x.Id == u.Id).Nome));
        }

        private static Faker<Usuario> CriarUsuarioFaker()
        {
            return new Faker<Usuario>()
                .UsePrivateConstructor()
                .RuleFor(u => u.Id, Guid.NewGuid())
                .RuleFor(u => u.Nome, f => f.Person.UserName)
                .RuleFor(u => u.Email, f => new Email(f.Person.Email))
                .RuleFor(u => u.HashSenha, f => f.Internet.Password(60));
        }

        private IUsuarioRepositorio CriarRepositorio()
            => new UsuarioRepositorio(_fixture.Context);

        private IUnitOfWork CriarUoW()
            => new UnitOfWork(_fixture.Context, Mock.Of<ILogger<UnitOfWork>>());

        private async Task<(IUsuarioRepositorio repositorio, IUnitOfWork unitOfWork, IEnumerable<Usuario> usuario)> InserirUsuarios(int quantidadeUsuarios)
        {
            var uow = CriarUoW();
            var repositorio = CriarRepositorio();

            var usuarios = CriarUsuarioFaker().Generate(quantidadeUsuarios);

            repositorio.AddRange(usuarios);
            await uow.SaveChangesAsync();

            return (repositorio, uow, usuarios);
        }
    }
}
