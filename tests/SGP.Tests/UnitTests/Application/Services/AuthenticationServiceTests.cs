using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using FluentResults.Extensions.FluentAssertions;
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
using SGP.Shared.Interfaces;
using SGP.Tests.Constants;
using SGP.Tests.Fixtures;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Application.Services
{
    [UnitTest(TestCategories.Application)]
    public class AuthenticationServiceTests : IClassFixture<EfSqliteFixture>
    {
        private static readonly Faker Faker = new();
        private readonly EfSqliteFixture _fixture;

        public AuthenticationServiceTests(EfSqliteFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Devera_RetornarTokenResponse_AoAutenticar()
        {
            // Arrange
            var authConfigOptions = CreateAuthConfigOptions();
            var jwtConfigOptions = CreateJwtConfigOptions();
            IDateTime dateTime = new LocalDateTimeService();
            IHashService hashService = new BCryptHashService(Mock.Of<ILogger<BCryptHashService>>());
            ITokenClaimsService tokenClaimService = new IdentityTokenClaimService(jwtConfigOptions, dateTime);
            IUsuarioRepository repository = new UsuarioRepository(_fixture.Context);
            IUnitOfWork unitOfWork = new UnitOfWork(_fixture.Context, Mock.Of<ILogger<UnitOfWork>>());
            IAuthenticationService service = new AuthenticationService(
                authConfigOptions,
                dateTime,
                hashService,
                tokenClaimService,
                repository,
                unitOfWork);

            var userName = Faker.Person.FullName;
            var email = Faker.Person.Email;
            var senha = Faker.Internet.Password(8);
            var usuario = new Usuario(userName, new Email(email), hashService.Hash(senha));
            repository.Add(usuario);
            await unitOfWork.SaveChangesAsync();

            var request = new LogInRequest(email, senha);

            // Act
            var act = await service.AuthenticateAsync(request);

            // Assert
            act.Should().NotBeNull().And.BeSuccess();
            act.Value.AccessToken.Should().NotBeNullOrWhiteSpace();
            act.Value.Expiration.Should().BeAfter(act.Value.Created);
            act.Value.RefreshToken.Should().NotBeNullOrWhiteSpace();
            act.Value.ExpiresIn.Should().BePositive().And.Be(jwtConfigOptions.Value.Seconds);
        }

        private static IOptions<AuthConfig> CreateAuthConfigOptions()
        {
            const short maximumAttempts = 3;
            const short secondsBlocked = 1000;
            return Options.Create(AuthConfig.Create(maximumAttempts, secondsBlocked));
        }

        private static IOptions<JwtConfig> CreateJwtConfigOptions()
        {
            const string audience = "Clients-API-SGP";
            const string issuer = "API-SGP";
            const short seconds = 21600;
            var secret = Faker.Random.String2(32);
            return Options.Create(JwtConfig.Create(audience, issuer, seconds, secret, true, true));
        }
    }
}