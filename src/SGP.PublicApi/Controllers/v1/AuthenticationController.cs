using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.Application.Interfaces;
using SGP.Application.Requests.AuthenticationRequests;
using SGP.PublicApi.Extensions;

namespace SGP.PublicApi.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _service;

        public AuthenticationController(IAuthenticationService service) => _service = service;

        /// <summary>
        /// Autenticar um usuário - AUTH01
        /// </summary>
        /// <param name="request">Endereço de e-mail e senha.</param>
        /// <response code="200">Retorna o token de acesso.</response>
        /// <response code="400">Retorna lista de erros, se a requisição for inválida.</response>
        /// <response code="404">Quando nenhuma conta é encontrado pelo e-mail e senha fornecido.</response>
        [HttpPost("authenticate")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Authenticate([FromBody] LogInRequest request)
        {
            var result = await _service.AuthenticateAsync(request);
            return result.ToHttpResult();
        }

        /// <summary>
        /// Atualiza um token de acesso - AUTH02
        /// </summary>
        /// <param name="request">O Token de atualização (RefreshToken).</param>
        /// <response code="200">Retorna um novo token de acesso.</response>
        /// <response code="400">Retorna lista de erros, se a requisição for inválida.</response>
        /// <response code="404">Quando nenhum token de acesso é encontrado.</response>
        [HttpPost("refresh-token")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var result = await _service.RefreshTokenAsync(request);
            return result.ToHttpResult();
        }
    }
}