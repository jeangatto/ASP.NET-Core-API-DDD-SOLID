using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SGP.Application.Requests.AuthenticationRequests;
using SGP.Application.Responses;
using SGP.Domain.Entities;
using SGP.Domain.ValueObjects;
using SGP.Infrastructure.Data.Context;
using SGP.Shared.Abstractions;
using SGP.Shared.Extensions;
using SGP.Tests.Extensions;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.IntegrationTests.Controllers;

[IntegrationTest]
public class AuthenticationControllerTests
{
    [Fact]
    public async Task Devera_RetornarResultadoSucessoComToken_AoAutenticar()
    {
        // Arrange
        const string endpoint = "/api/auth/authenticate";
        const string nome = "Jean Gatto";
        const string email = "jean_gatto@hotmail.com";
        const string senha = "@JiL8@cUA%pV";

        await using var webApplicationFactory = CreateWebApplication((serviceScope) =>
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<SgpContext>();
            var hashService = serviceScope.ServiceProvider.GetRequiredService<IHashService>();
            var usuario = new Usuario(nome, new Email(email), hashService.Hash(senha));
            context.Add(usuario);
            context.SaveChanges();
        });

        using var httpClient = webApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        var request = new LogInRequest(email, senha);
        using var httpContent = new StringContent(request.ToJson(), Encoding.UTF8, MediaTypeNames.Application.Json);

        // Act
        var act = await httpClient.PostAsync<TokenResponse>(endpoint, httpContent);

        // Assert
        act.Should().NotBeNull();
        act.AccessToken.Should().NotBeNullOrWhiteSpace();
        act.RefreshToken.Should().NotBeNullOrWhiteSpace();
        act.ExpiresIn.Should().BePositive();
        act.Expiration.Should().BeAfter(act.Created);
        act.Created.Should().BeSameDateAs(DateTime.Now);
    }

    private static WebApplicationFactory<Program> CreateWebApplication(Action<IServiceScope> configureServiceScope)
    {
        var factory = new WebApplicationFactory<Program>()
             .WithWebHostBuilder(hostBuilder => hostBuilder.ConfigureLogging(logging => logging.ClearProviders()));

        using var serviceScope = factory.Services.CreateScope();

        configureServiceScope.Invoke(serviceScope);

        return factory;
    }
}