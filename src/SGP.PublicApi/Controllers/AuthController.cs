using System.Net.Mime;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.Application.Interfaces;
using SGP.Application.Requests.AuthenticationRequests;
using SGP.Application.Responses;
using SGP.PublicApi.Extensions;
using SGP.PublicApi.Models;

namespace SGP.PublicApi.Controllers;

[Route("api/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class AuthController(IAuthenticationService authenticationService) : ControllerBase
{
    /// <summary>
    /// Efetua a autenticação.
    /// </summary>
    /// <param name="request">Endereço de e-mail e senha.</param>
    /// <response code="200">Retorna o token de acesso.</response>
    /// <response code="400">Retorna lista de erros, se a requisição for inválida.</response>
    /// <response code="404">Quando nenhuma conta é encontrado pelo e-mail e senha fornecido.</response>
    [HttpPost("authenticate")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResponse<TokenResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Authenticate([FromBody] LogInRequest request) =>
        (await authenticationService.AuthenticateAsync(request)).ToActionResult();

    /// <summary>
    /// Atualiza um token de acesso.
    /// </summary>
    /// <param name="request">O Token de atualização (RefreshToken).</param>
    /// <response code="200">Retorna um novo token de acesso.</response>
    /// <response code="400">Retorna lista de erros, se a requisição for inválida.</response>
    /// <response code="401">Sem autorização.</response>
    /// <response code="404">Quando nenhum token de acesso é encontrado.</response>
    [HttpPost("refresh-token")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResponse<TokenResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request) =>
        (await authenticationService.RefreshTokenAsync(request)).ToActionResult();
}