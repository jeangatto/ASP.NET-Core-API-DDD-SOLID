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
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Infrastructure.Repositories
{
    [Category(TestCategories.Infrastructure)]
    public class UsuarioRepositorioTests : UnitTestBase, IClassFixture<EfSqliteFixture>
    {
        private readonly EfSqliteFixture _fixture;

        public UsuarioRepositorioTests(EfSqliteFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Devera_RetonarVerdadeiro_QuandoVerificarSeEmailJaExiste()
        {
            // Arrange
            var (repositorio, usuario) = await InserirUsuario();

            // Act
            var actual = await repositorio.VerificaSeEmailExisteAsync(usuario.Email);

            // Assert
            actual.Should().Be(true);
        }

        [Fact]
        public async Task Devera_RetornarUsuario_QuandoObterPorEmail()
        {
            // Arrange
            var (repositorio, usuario) = await InserirUsuario();

            // Act
            var actual = await repositorio.ObterPorEmailAsync(usuario.Email);

            // Assert
            actual.Should().NotBeNull();
            actual.Id.Should().NotBeEmpty().And.Be(usuario.Id);
            actual.Nome.Should().NotBeNullOrWhiteSpace().And.Be(usuario.Nome);
            actual.Email.Should().NotBeNull().And.Be(usuario.Email);
            actual.HashSenha.Should().NotBeNullOrWhiteSpace().And.Be(usuario.HashSenha);
            actual.Tokens.Should().NotBeEmpty().And.HaveCount(usuario.Tokens.Count).And.Subject.ForEach(t =>
            {
                t.Id.Should().NotBeEmpty();
                t.UsuarioId.Should().Be(usuario.Id);
                t.Token.Should().NotBeNullOrWhiteSpace();
                t.CriadoEm.Should().BeBefore(t.ExpiraEm);
                t.ExpiraEm.Should().BeAfter(t.CriadoEm);
            });
        }

        [Fact]
        public async Task Devera_RetornarUsuario_QuandoObterPorId()
        {
            // Arrange
            var (repositorio, usuario) = await InserirUsuario();

            // Act
            var actual = await repositorio.GetByIdAsync(usuario.Id);

            // Assert
            actual.Should().NotBeNull();
            actual.Id.Should().NotBeEmpty().And.Be(usuario.Id);
            actual.Nome.Should().NotBeNullOrWhiteSpace().And.Be(usuario.Nome);
            actual.Email.Should().NotBeNull().And.Be(usuario.Email);
            actual.HashSenha.Should().NotBeNullOrWhiteSpace().And.Be(usuario.HashSenha);
            actual.Tokens.Should().NotBeEmpty().And.HaveCount(usuario.Tokens.Count).And.Subject.ForEach(t =>
            {
                t.Id.Should().NotBeEmpty();
                t.UsuarioId.Should().Be(usuario.Id);
                t.Token.Should().NotBeNullOrWhiteSpace();
                t.CriadoEm.Should().BeBefore(t.ExpiraEm);
                t.ExpiraEm.Should().BeAfter(t.CriadoEm);
            });
        }

        [Fact]
        public async Task Devera_RetornarUsuario_QuandoObterPorToken()
        {
            // Arrange
            var (repositorio, usuario) = await InserirUsuario(3);
            var token = usuario.Tokens[0];

            // Act
            var actual = await repositorio.ObterPorTokenAsync(token.Token);

            // Assert
            actual.Should().NotBeNull();
            actual.Id.Should().NotBeEmpty().And.Be(usuario.Id);
            actual.Nome.Should().NotBeNullOrWhiteSpace().And.Be(usuario.Nome);
            actual.Email.Should().NotBeNull().And.Be(usuario.Email);
            actual.HashSenha.Should().NotBeNullOrWhiteSpace().And.Be(usuario.HashSenha);
            actual.Tokens.Should().NotBeEmpty().And.HaveCount(1).And.Subject.ForEach(t =>
            {
                t.Id.Should().Be(token.Id);
                t.Token.Should().Be(token.Token);
                t.CriadoEm.Should().Be(token.CriadoEm);
                t.ExpiraEm.Should().Be(token.ExpiraEm);
            });
        }

        private static Usuario CriarUsuario(int quantidadeTokens)
        {
            var usuarioId = Guid.NewGuid();

            var tokens = new Faker<TokenAcesso>()
                .UsePrivateConstructor()
                .RuleFor(t => t.Id, Guid.NewGuid())
                .RuleFor(t => t.UsuarioId, usuarioId)
                .RuleFor(t => t.Token, f => f.Internet.Password(2048))
                .RuleFor(t => t.CriadoEm, f => f.Date.Soon())
                .RuleFor(t => t.ExpiraEm, f => f.Date.Soon(1))
                .Generate(quantidadeTokens);

            return new Faker<Usuario>()
                .UsePrivateConstructor()
                .RuleFor(u => u.Id, usuarioId)
                .RuleFor(u => u.Nome, f => f.Person.UserName)
                .RuleFor(u => u.Email, f => new Email(f.Person.Email))
                .RuleFor(u => u.HashSenha, f => f.Internet.Password(60))
                .FinishWith((_, u) =>
                {
                    foreach (var token in tokens)
                    {
                        u.AdicionarToken(token);
                    }
                })
                .Generate();
        }

        private IUsuarioRepositorio CriarRepositorio()
            => new UsuarioRepositorio(_fixture.Context);

        private IUnitOfWork CriarUoW()
            => new UnitOfWork(_fixture.Context, Mock.Of<ILogger<UnitOfWork>>());

        private async Task<(IUsuarioRepositorio, Usuario)> InserirUsuario(int quantidadeTokens = 1)
        {
            var uow = CriarUoW();
            var repositorio = CriarRepositorio();

            var usuario = CriarUsuario(quantidadeTokens);

            repositorio.Add(usuario);
            await uow.SaveChangesAsync();

            return (repositorio, usuario);
        }
    }
}