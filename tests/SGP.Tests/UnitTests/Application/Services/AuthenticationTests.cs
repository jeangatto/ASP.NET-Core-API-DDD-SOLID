using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SGP.Application.Requests.AuthenticationRequests;
using SGP.Application.Services;
using SGP.Domain.Entities;
using SGP.Domain.ValueObjects;
using SGP.Infrastructure.Repositories;
using SGP.Infrastructure.Services;
using SGP.Infrastructure.UoW;
using SGP.Shared.Errors;
using SGP.Tests.Constants;
using SGP.Tests.DataFakers;
using SGP.Tests.Extensions;
using SGP.Tests.Fixtures;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Application.Services
{
    [UnitTest(TestCategories.Application)]
    public class AuthenticationTests : IClassFixture<EfSqliteFixture>
    {
        private readonly EfSqliteFixture _fixture;

        public AuthenticationTests(EfSqliteFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Devera_RetornarSucessoComToken_AoAutenticar()
        {
            // Arrange
            var authConfigOptions = OptionsDataFaker.AuthConfigOptions;
            var jwtConfigOptions = OptionsDataFaker.JwtConfigOptions;
            var dateTime = new LocalDateTimeService();
            var tokenClaimService = new IdentityTokenClaimService(jwtConfigOptions, dateTime);
            var hashService = new BCryptHashService(Mock.Of<ILogger<BCryptHashService>>());
            var repository = new UsuarioRepository(_fixture.Context);
            var unitOfWork = new UnitOfWork(_fixture.Context, Mock.Of<ILogger<UnitOfWork>>());
            var service = new AuthenticationService(
                authConfigOptions,
                dateTime,
                hashService,
                tokenClaimService,
                repository,
                unitOfWork);

            const string nome = "Gatto";
            const string email = "jean_gatto@hotmail.com";
            const string senha = "VWBMx1bVqP01";
            repository.Add(new Usuario(nome, new Email(email), hashService.Hash(senha)));
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
            tokenResponse.Expiration.Should().BeAfter(actual.Value.Created);
            tokenResponse.RefreshToken.Should().NotBeNullOrWhiteSpace();
            tokenResponse.ExpiresIn.Should().BePositive().And.Be(jwtConfigOptions.Value.Seconds);
        }

        [Fact]
        public async Task Devera_RetornarErroValidacao_AoAutenticarLogInInvalido()
        {
            // Arrange
            var authConfigOptions = OptionsDataFaker.AuthConfigOptions;
            var jwtConfigOptions = OptionsDataFaker.JwtConfigOptions;
            var dateTime = new LocalDateTimeService();
            var tokenClaimService = new IdentityTokenClaimService(jwtConfigOptions, dateTime);
            var hashService = new BCryptHashService(Mock.Of<ILogger<BCryptHashService>>());
            var repository = new UsuarioRepository(_fixture.Context);
            var unitOfWork = new UnitOfWork(_fixture.Context, Mock.Of<ILogger<UnitOfWork>>());
            var service = new AuthenticationService(
                authConfigOptions,
                dateTime,
                hashService,
                tokenClaimService,
                repository,
                unitOfWork);

            var request = new LogInRequest(string.Empty, string.Empty);

            // Act
            var actual = await service.AuthenticateAsync(request);

            // Assert
            actual.Should().NotBeNull();
            actual.HasError<ValidationError>().Should().BeTrue();
            actual.IsFailed.Should().BeTrue();
            actual.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.Subject.ForEach(error => error.Message.Should().NotBeNullOrEmpty());
        }

        [Fact]
        public async Task Devera_RetornarErroValidacao_AoAutenticarLogInInexistente()
        {
            // Arrange
            const string expectedError = "A conta informada não existe.";
            var authConfigOptions = OptionsDataFaker.AuthConfigOptions;
            var jwtConfigOptions = OptionsDataFaker.JwtConfigOptions;
            var dateTime = new LocalDateTimeService();
            var tokenClaimService = new IdentityTokenClaimService(jwtConfigOptions, dateTime);
            var hashService = new BCryptHashService(Mock.Of<ILogger<BCryptHashService>>());
            var repository = new UsuarioRepository(_fixture.Context);
            var unitOfWork = new UnitOfWork(_fixture.Context, Mock.Of<ILogger<UnitOfWork>>());
            var service = new AuthenticationService(
                authConfigOptions,
                dateTime,
                hashService,
                tokenClaimService,
                repository,
                unitOfWork);

            var request = new LogInRequest("joao.ninguem@gmail.com", "gUoCA3#d1oKB");

            // Act
            var actual = await service.AuthenticateAsync(request);

            // Assert
            actual.Should().NotBeNull();
            actual.HasError<NotFoundError>().Should().BeTrue();
            actual.IsFailed.Should().BeTrue();
            actual.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.SatisfyRespectively(error => error.Message.Should().NotBeNullOrWhiteSpace().And.Be(expectedError));
        }
    }
}