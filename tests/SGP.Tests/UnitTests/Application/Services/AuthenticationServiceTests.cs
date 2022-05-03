using System;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SGP.Application.Interfaces;
using SGP.Application.Requests.AuthenticationRequests;
using SGP.Application.Services;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Domain.ValueObjects;
using SGP.Infrastructure.Repositories;
using SGP.Infrastructure.Services;
using SGP.Infrastructure.UoW;
using SGP.Shared.AppSettings;
using SGP.Shared.Errors;
using SGP.Shared.Interfaces;
using SGP.Tests.Extensions;
using SGP.Tests.Fixtures;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Application.Services;

[UnitTest]
public class AuthenticationServiceTests : IClassFixture<EfSqliteFixture>
{
    private readonly EfSqliteFixture _fixture;

    public AuthenticationServiceTests(EfSqliteFixture fixture) => _fixture = fixture;

    [Fact]
    public async Task Devera_RetornarSucessoComToken_AoAutenticar()
    {
        // Arrange
        var jwtConfig = CreateJwtConfig();
        var dateTime = new DateTimeService();
        var tokenClaimsService = new JwtClaimService(jwtConfig, dateTime);
        var hashService = new BCryptHashService(Mock.Of<ILogger<BCryptHashService>>());
        var usuarioRepository = new UsuarioRepository(_fixture.Context);
        var unitOfWork = new UnitOfWork(_fixture.Context, Mock.Of<ILogger<UnitOfWork>>());
        var service = CreateAuthenticationService(
            CreateAuthConfig(),
            dateTime,
            hashService,
            tokenClaimsService,
            usuarioRepository,
            unitOfWork);

        const string nome = "Gatto";
        const string email = "jean_gatto@hotmail.com";
        const string senha = "VWBMx1bVqP01";
        usuarioRepository.Add(new Usuario(nome, new Email(email), hashService.Hash(senha)));
        await unitOfWork.SaveChangesAsync();
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
        tokenResponse.ExpiresIn.Should().BePositive().And.Be(jwtConfig.Value.Seconds);
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

        var usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        usuarioRepositoryMock
            .Setup(s => s.ObterPorEmailAsync(It.IsNotNull<Email>()))
            .ReturnsAsync(usuario);

        var dateTimeMock = new Mock<IDateTimeService>();
        dateTimeMock.SetupGet(s => s.Now).Returns(DateTime.Now);

        var service = CreateAuthenticationService(
            dateTimeService: dateTimeMock.Object,
            usuarioRepository: usuarioRepositoryMock.Object);

        // Act
        var actual = await service.AuthenticateAsync(logInRequest);

        // Assert
        actual.Should().NotBeNull();
        actual.IsFailed.Should().BeTrue();
        actual.Errors.Should().NotBeNullOrEmpty()
            .And.OnlyHaveUniqueItems()
            .And.SatisfyRespectively(error => error.Message.Should().NotBeNullOrWhiteSpace().And.Be(expectedError));

        usuarioRepositoryMock.Verify(s => s.ObterPorEmailAsync(It.IsNotNull<Email>()), Times.Once);
        dateTimeMock.Verify(s => s.Now, Times.Once);
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
        actual.HasError<ValidationError>().Should().BeTrue();
        actual.IsFailed.Should().BeTrue();
        actual.Errors.Should().NotBeNullOrEmpty()
            .And.OnlyHaveUniqueItems()
            .And.Subject.ForEach(error => error.Message.Should().NotBeNullOrWhiteSpace());
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
        actual.HasError<NotFoundError>().Should().BeTrue();
        actual.IsFailed.Should().BeTrue();
        actual.Errors.Should().NotBeNullOrEmpty()
            .And.OnlyHaveUniqueItems()
            .And.SatisfyRespectively(error => error.Message.Should().NotBeNullOrWhiteSpace().And.Be(expectedError));
    }

    private static IAuthenticationService CreateAuthenticationService(
        IOptions<AuthConfig> authOptions = null,
        IDateTimeService dateTimeService = null,
        IHashService hashService = null,
        ITokenClaimsService tokenClaimsService = null,
        IUsuarioRepository usuarioRepository = null,
        IUnitOfWork unitOfWork = null)
    {
        return new AuthenticationService(
            authOptions ?? Mock.Of<IOptions<AuthConfig>>(),
            dateTimeService ?? Mock.Of<IDateTimeService>(),
            hashService ?? Mock.Of<IHashService>(),
            tokenClaimsService ?? Mock.Of<ITokenClaimsService>(),
            usuarioRepository ?? Mock.Of<IUsuarioRepository>(),
            unitOfWork ?? Mock.Of<IUnitOfWork>());
    }

    private static IOptions<AuthConfig> CreateAuthConfig()
    {
        const short maximumAttempts = 3;
        const short secondsBlocked = 1000;
        return Options.Create(AuthConfig.Create(maximumAttempts, secondsBlocked));
    }

    private static IOptions<JwtConfig> CreateJwtConfig()
    {
        const string audience = "Clients-API-SGP";
        const string issuer = "API-SGP";
        const string secretKey = "p8SXNddEAEn1cCuyfVJKYA7e6hlagbLd";
        const short seconds = 21600;
        var jwtConfig = JwtConfig.Create(audience, issuer, seconds, secretKey);
        return Options.Create(jwtConfig);
    }
}