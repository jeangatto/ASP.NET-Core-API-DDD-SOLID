using System;
using System.Threading.Tasks;
using Ardalis.Result;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using SGP.Application.Requests.AuthenticationRequests;
using SGP.Application.Services;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Domain.ValueObjects;
using SGP.Infrastructure.Data;
using SGP.Infrastructure.Data.Repositories;
using SGP.Infrastructure.Services;
using SGP.Shared.Abstractions;
using SGP.Shared.AppSettings;
using SGP.Tests.Extensions;
using SGP.Tests.Fixtures;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Application.Services;

[UnitTest]
public class AuthenticationServiceTests(EfSqliteFixture fixture) : IClassFixture<EfSqliteFixture>
{
    private readonly EfSqliteFixture _fixture = fixture;

    [Fact]
    public async Task Devera_RetornarSucessoComToken_AoAutenticar()
    {
        // Arrange
        var jwtOptions = CreateJwtOptions();
        var dateTime = new DateTimeService();
        var tokenClaimsService = new JwtClaimService(jwtOptions, dateTime);
        var hashService = new BCryptHashService(Substitute.For<ILogger<BCryptHashService>>());
        var usuarioRepository = new UsuarioRepository(_fixture.Context);
        var unitOfWork = new UnitOfWork(_fixture.Context, Substitute.For<ILogger<UnitOfWork>>());
        var service = CreateAuthenticationService(
            CreateAuthOptions(),
            dateTime,
            hashService,
            tokenClaimsService,
            usuarioRepository,
            unitOfWork);

        const string nome = "Gatto";
        const string email = "jean_gatto@hotmail.com";
        const string senha = "VWBMx1bVqP01";
        usuarioRepository.Add(new Usuario(nome, new Email(email), hashService.Hash(senha)));
        await unitOfWork.CommitAsync();
        var request = new LogInRequest(email, senha);

        // Act
        var actual = await service.AuthenticateAsync(request);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeTrue();
        actual.Value.Should().NotBeNull();
        var tokenResponse = actual.Value;
        tokenResponse.AccessToken.Should().NotBeNullOrWhiteSpace();
        tokenResponse.Expiration.Should().BeAfter(tokenResponse.Created);
        tokenResponse.RefreshToken.Should().NotBeNullOrWhiteSpace();
        tokenResponse.ExpiresIn.Should().BePositive().And.Be(jwtOptions.Value.Seconds);
    }

    [Fact]
    public async Task Devera_RetornarErroValidacao_AoAutenticarContaBloqueada()
    {
        // Arrange
        const string expectedError = "A sua conta está bloqueada, entre em contato com o nosso suporte.";

        var usuario = new Faker<Usuario>()
            .UsePrivateConstructor()
            .RuleFor(usuario => usuario.Id, faker => faker.Random.Guid())
            .RuleFor(usuario => usuario.Nome, faker => faker.Person.FullName)
            .RuleFor(usuario => usuario.Email, faker => new Email(faker.Person.Email))
            .RuleFor(usuario => usuario.HashSenha, faker => faker.Internet.Password())
            .RuleFor(usuario => usuario.UltimoAcessoEm, faker => faker.Date.Past())
            .RuleFor(usuario => usuario.BloqueioExpiraEm, faker => faker.Date.Future())
            .RuleFor(usuario => usuario.NumeroFalhasAoAcessar, faker => faker.Random.Int(1, 10))
            .Generate();

        var logInRequest = new LogInRequest(usuario.Email.Address, usuario.HashSenha);

        var usuarioRepository = Substitute.For<IUsuarioRepository>();
        usuarioRepository.ObterPorEmailAsync(Arg.Is<Email>(email => email == usuario.Email)).Returns(usuario);

        var dateTimeService = Substitute.For<IDateTimeService>();
        dateTimeService.Now.Returns(DateTime.Now);

        var service = CreateAuthenticationService(
            dateTimeService: dateTimeService,
            usuarioRepository: usuarioRepository);

        // Act
        var actual = await service.AuthenticateAsync(logInRequest);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeFalse();
        actual.Status.Should().Be(ResultStatus.Error);
        actual.Errors.Should().NotBeNullOrEmpty()
            .And.OnlyHaveUniqueItems()
            .And.SatisfyRespectively(error => error.Should().NotBeNullOrWhiteSpace().And.Be(expectedError));

        await usuarioRepository.Received(1).ObterPorEmailAsync(Arg.Any<Email>());
    }

    [Fact]
    public async Task Devera_RetornarErroValidacao_AoAutenticarLogInInvalido()
    {
        // Arrange
        var logInRequest = new LogInRequest(string.Empty, string.Empty);
        var service = CreateAuthenticationService();

        // Act
        var actual = await service.AuthenticateAsync(logInRequest);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeFalse();
        actual.Status.Should().Be(ResultStatus.Invalid);
        actual.ValidationErrors.Should().NotBeNullOrEmpty()
            .And.OnlyHaveUniqueItems()
            .And.Subject.ForEach(error => error.ErrorMessage.Should().NotBeNullOrWhiteSpace());
    }

    [Fact]
    public async Task Devera_RetornarErroValidacao_AoAutenticarLogInInexistente()
    {
        // Arrange
        const string expectedError = "A conta informada não existe.";
        var logInRequest = new LogInRequest("joao.ninguem@gmail.com", "gUoCA3#d1oKB");
        var service = CreateAuthenticationService();

        // Act
        var actual = await service.AuthenticateAsync(logInRequest);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccess.Should().BeFalse();
        actual.Status.Should().Be(ResultStatus.NotFound);
        actual.Errors.Should().NotBeNullOrEmpty()
            .And.OnlyHaveUniqueItems()
            .And.SatisfyRespectively(error => error.Should().NotBeNullOrWhiteSpace().And.Be(expectedError));
    }

    private static AuthenticationService CreateAuthenticationService(
        IOptions<AuthOptions> authOptions = null,
        IDateTimeService dateTimeService = null,
        IHashService hashService = null,
        ITokenClaimsService tokenClaimsService = null,
        IUsuarioRepository usuarioRepository = null,
        IUnitOfWork unitOfWork = null)
    {
        return new AuthenticationService(
            authOptions ?? Substitute.For<IOptions<AuthOptions>>(),
            dateTimeService ?? Substitute.For<IDateTimeService>(),
            hashService ?? Substitute.For<IHashService>(),
            tokenClaimsService ?? Substitute.For<ITokenClaimsService>(),
            usuarioRepository ?? Substitute.For<IUsuarioRepository>(),
            unitOfWork ?? Substitute.For<IUnitOfWork>());
    }

    private static IOptions<AuthOptions> CreateAuthOptions()
    {
        const short maximumAttempts = 3;
        const short secondsBlocked = 1000;
        return Options.Create(AuthOptions.Create(maximumAttempts, secondsBlocked));
    }

    private static IOptions<JwtOptions> CreateJwtOptions()
    {
        const string audience = "Clients-API-SGP";
        const string issuer = "API-SGP";
        const string secretKey = "p8SXNddEAEn1cCuyfVJKYA7e6hlagbLd";
        const short seconds = 21600;
        var jwtConfig = JwtOptions.Create(audience, issuer, seconds, secretKey);
        return Options.Create(jwtConfig);
    }
}