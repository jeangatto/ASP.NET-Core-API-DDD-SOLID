using System.Threading.Tasks;
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
using SGP.Shared.Interfaces;
using SGP.Tests.Constants;
using SGP.Tests.Extensions;
using SGP.Tests.Fixtures;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Application.Services
{
    [UnitTest(TestCategories.Application)]
    public class AuthenticationTests : UnitTestBase, IClassFixture<EfSqliteFixture>
    {
        private readonly EfSqliteFixture _fixture;

        public AuthenticationTests(EfSqliteFixture fixture) => _fixture = fixture;

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

            var name = Faker.Person.FullName;
            var email = Faker.Person.Email;
            var senha = Faker.Internet.Password(8);
            var usuario = new Usuario(name, new Email(email), hashService.Hash(senha));
            repository.Add(usuario);
            await unitOfWork.SaveChangesAsync();

            var request = new LogInRequest(email, senha);

            // Act
            var act = await service.AuthenticateAsync(request);

            // Assert
            act.Should().NotBeNull();
            act.IsSuccess.Should().BeTrue();
            act.Value.AccessToken.Should().NotBeNullOrWhiteSpace();
            act.Value.Expiration.Should().BeAfter(act.Value.Created);
            act.Value.RefreshToken.Should().NotBeNullOrWhiteSpace();
            act.Value.ExpiresIn.Should().BePositive().And.Be(jwtConfigOptions.Value.Seconds);
        }

        [Fact]
        public async Task Devera_RetornarErroValidacao_AoAutenticarLogInInvalido()
        {
            // Arrange
            IAuthenticationService service = new AuthenticationService(
                Mock.Of<IOptions<AuthConfig>>(),
                Mock.Of<IDateTime>(),
                Mock.Of<IHashService>(),
                Mock.Of<ITokenClaimsService>(),
                Mock.Of<IUsuarioRepository>(),
                Mock.Of<IUnitOfWork>());

            var request = new LogInRequest(string.Empty, string.Empty);

            // Act
            var act = await service.AuthenticateAsync(request);

            // Assert
            act.Should().NotBeNull();
            act.IsFailed.Should().BeTrue();
            act.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.Subject.ForEach(error => error.Message.Should().NotBeNullOrEmpty());
        }
    }
}