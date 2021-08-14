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
using SGP.Tests.Constants;
using SGP.Tests.Fixtures;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.UnitTests.Application.Services
{
    [UnitTest(TestCategories.Application)]
    public class AuthenticationTestTests : TestBase, IClassFixture<EfSqliteFixture>
    {
        private readonly EfSqliteFixture _fixture;

        public AuthenticationTestTests(EfSqliteFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Devera_RetornarTokenResponse_AoAutenticar()
        {
            // Arrange
            var authConfigOptions = CreateAuthConfigOptions();
            var jwtConfigOptions = CreateJwtConfigOptions();
            var dateTime = new LocalDateTimeService();
            var hashService = new BCryptHashService(Mock.Of<ILogger<BCryptHashService>>());
            var tokenClaimService = new IdentityTokenClaimService(jwtConfigOptions, dateTime);
            var repository = new UsuarioRepository(_fixture.Context);
            var unitOfWork = new UnitOfWork(_fixture.Context, Mock.Of<ILogger<UnitOfWork>>());
            var service = new AuthenticationService(
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
    }
}