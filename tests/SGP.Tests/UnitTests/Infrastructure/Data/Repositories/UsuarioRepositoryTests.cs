using System;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Domain.ValueObjects;
using SGP.Infrastructure.Data;
using SGP.Infrastructure.Data.Repositories;
using SGP.Tests.Extensions;
using SGP.Tests.Fixtures;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Infrastructure.Data.Repositories;

[UnitTest]
public class UsuarioRepositoryTests(EfSqliteFixture fixture) : IClassFixture<EfSqliteFixture>
{
    [Fact]
    public async Task Devera_RetonarVerdadeiro_AoVerificarSeEmailJaExiste()
    {
        // Arrange
        var (repositorio, usuarioInserido) = await PopularAsync();

        // Act
        var actual = await repositorio.VerificarSeEmailExisteAsync(usuarioInserido.Email);

        // Assert
        actual.Should().Be(true);
    }

    [Fact]
    public async Task Devera_RetornarUsuario_AoObterPorEmail()
    {
        // Arrange
        var (repositorio, usuarioInserido) = await PopularAsync();

        // Act
        var actual = await repositorio.ObterPorEmailAsync(usuarioInserido.Email);

        // Assert
        actual.Should().NotBeNull();
        actual.Id.Should().NotBeEmpty().And.Be(usuarioInserido.Id);
        actual.Nome.Should().NotBeNullOrWhiteSpace().And.Be(usuarioInserido.Nome);
        actual.Email.Should().NotBeNull().And.Be(usuarioInserido.Email);
        actual.HashSenha.Should().NotBeNullOrWhiteSpace().And.Be(usuarioInserido.HashSenha);
        actual.Tokens.Should().NotBeEmpty().And.HaveCount(usuarioInserido.Tokens.Count)
            .And.Subject.ForEach(t =>
            {
                t.Id.Should().NotBeEmpty();
                t.UsuarioId.Should().Be(usuarioInserido.Id);
                t.Acesso.Should().NotBeNullOrWhiteSpace();
                t.Atualizacao.Should().NotBeNullOrWhiteSpace();
                t.CriadoEm.Should().BeBefore(t.ExpiraEm);
                t.ExpiraEm.Should().BeAfter(t.CriadoEm);
            });
    }

    [Fact]
    public async Task Devera_RetornarUsuario_AoObterPorId()
    {
        // Arrange
        var (repositorio, usuarioInserido) = await PopularAsync();

        // Act
        var actual = await repositorio.GetByIdAsync(usuarioInserido.Id);

        // Assert
        actual.Should().NotBeNull();
        actual.Id.Should().NotBeEmpty().And.Be(usuarioInserido.Id);
        actual.Nome.Should().NotBeNullOrWhiteSpace().And.Be(usuarioInserido.Nome);
        actual.Email.Should().NotBeNull().And.Be(usuarioInserido.Email);
        actual.HashSenha.Should().NotBeNullOrWhiteSpace().And.Be(usuarioInserido.HashSenha);
        actual.Tokens.Should().NotBeEmpty().And.HaveCount(usuarioInserido.Tokens.Count)
            .And.Subject.ForEach(t =>
            {
                t.Id.Should().NotBeEmpty();
                t.UsuarioId.Should().Be(usuarioInserido.Id);
                t.Acesso.Should().NotBeNullOrWhiteSpace();
                t.Atualizacao.Should().NotBeNullOrWhiteSpace();
                t.CriadoEm.Should().BeBefore(t.ExpiraEm);
                t.ExpiraEm.Should().BeAfter(t.CriadoEm);
            });
    }

    [Fact]
    public async Task Devera_RetornarUsuario_AoObterPorToken()
    {
        // Arrange
        var (repositorio, usuarioInserido) = await PopularAsync(3);
        var tokenAtualizacao = usuarioInserido.Tokens[0].Atualizacao;

        // Act
        var actual = await repositorio.ObterPorTokenAtualizacaoAsync(tokenAtualizacao);

        // Assert
        actual.Should().NotBeNull();
        actual.Id.Should().NotBeEmpty().And.Be(usuarioInserido.Id);
        actual.Nome.Should().NotBeNullOrWhiteSpace().And.Be(usuarioInserido.Nome);
        actual.Email.Should().NotBeNull().And.Be(usuarioInserido.Email);
        actual.HashSenha.Should().NotBeNullOrWhiteSpace().And.Be(usuarioInserido.HashSenha);
        actual.Tokens.Should().NotBeEmpty().And.HaveCount(usuarioInserido.Tokens.Count)
            .And.Subject.ForEach(t =>
            {
                t.Id.Should().NotBeEmpty();
                t.UsuarioId.Should().Be(usuarioInserido.Id);
                t.Acesso.Should().NotBeNullOrWhiteSpace();
                t.Atualizacao.Should().NotBeNullOrWhiteSpace();
                t.CriadoEm.Should().BeBefore(t.ExpiraEm);
                t.ExpiraEm.Should().BeAfter(t.CriadoEm);
            });
    }

    private async Task<(IUsuarioRepository, Usuario)> PopularAsync(int quantidadeTokens = 1)
    {
        var usuario = CriarUsuario(quantidadeTokens);
        var repositorio = CriarRepositorio();
        repositorio.Add(usuario);
        var unitOfWork = CriarUoW();
        await unitOfWork.CommitAsync();
        return (repositorio, usuario);
    }

    private static Usuario CriarUsuario(int quantidadeTokens)
    {
        return new Faker<Usuario>()
            .UsePrivateConstructor()
            .RuleFor(usuario => usuario.Id, Guid.NewGuid())
            .RuleFor(usuario => usuario.Nome, faker => faker.Person.UserName)
            .RuleFor(usuario => usuario.Email, faker => new Email(faker.Person.Email))
            .RuleFor(usuario => usuario.HashSenha, faker => faker.Internet.Password(60))
            .FinishWith((faker, usuario) =>
            {
                for (var i = 0; i < quantidadeTokens; i++)
                {
                    var acesso = faker.Random.JsonWebToken();
                    var atualizacao = faker.Random.JsonWebToken();
                    var criadoEm = i == 0 ? DateTime.Now : DateTime.Now.AddDays(i + 1);
                    var expiraEm = criadoEm.AddHours(8);
                    usuario.AdicionarToken(new Token(acesso, atualizacao, criadoEm, expiraEm));
                }
            })
            .Generate();
    }

    private UsuarioRepository CriarRepositorio() => new(fixture.Context);

    private UnitOfWork CriarUoW() => new(fixture.Context, Substitute.For<ILogger<UnitOfWork>>());
}