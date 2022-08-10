using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SGP.Application.Requests.AuthenticationRequests;
using SGP.Application.Responses;
using SGP.Domain.Entities;
using SGP.Domain.ValueObjects;
using SGP.Infrastructure.Data.Context;
using SGP.Shared.Extensions;
using SGP.Shared.Interfaces;
using SGP.Tests.Extensions;
using SGP.Tests.Fixtures;
using Xunit;

namespace SGP.Tests.IntegrationTests.Controllers.v1;

public class AuthenticationControllerTests : IntegrationTestBase, IClassFixture<WebTestApplicationFactory>
{
    private const string SenhaPadrao = "@JiL8@cUA%pV";

    public AuthenticationControllerTests(WebTestApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Devera_RetornarResultadoSucessoComToken_AoAutenticar()
    {
        // Arrange
        const string endPoint = "/api/auth/authenticate";
        var usuario = await CriarUsuarioAsync();
        var request = new LogInRequest(usuario.Email.Address, SenhaPadrao);
        var httpContent = new StringContent(request.ToJson(), Encoding.UTF8, MediaTypeNames.Application.Json);

        // Act
        var act = await HttpClient.PostAsync<TokenResponse>(endPoint, httpContent);

        // Assert
        act.Should().NotBeNull();
        act.AccessToken.Should().NotBeNullOrWhiteSpace();
        act.RefreshToken.Should().NotBeNullOrWhiteSpace();
        act.ExpiresIn.Should().BePositive();
        act.Expiration.Should().BeAfter(act.Created);
        act.Created.Should().BeSameDateAs(DateTime.Now);
    }

    private async Task<Usuario> CriarUsuarioAsync()
    {
        await using var sp = ServiceProvider.CreateAsyncScope();
        await using var context = sp.ServiceProvider.GetRequiredService<SgpContext>();
        var hashService = sp.ServiceProvider.GetRequiredService<IHashService>();
        var usuario = new Usuario("Jean Gatto", new Email("jean_gatto@hotmail.com"), hashService.Hash(SenhaPadrao));
        context.Add(usuario);
        await context.SaveChangesAsync();
        return usuario;
    }
}