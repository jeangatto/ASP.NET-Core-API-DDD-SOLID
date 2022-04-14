using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.Application.Interfaces;
using SGP.Application.Requests.EstadoRequests;
using SGP.PublicApi.Extensions;

namespace SGP.PublicApi.Controllers.v1;

[Route("api/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class EstadosController : ControllerBase
{
    private readonly IEstadoService _service;

    public EstadosController(IEstadoService service) => _service = service;

    /// <summary>
    /// Obtém uma lista com todos os estados - EST01
    /// </summary>
    /// <response code="200">Retorna a lista de estados.</response>
    /// <response code="400">Retorna lista de erros, se a requisição for inválida.</response>
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ObterTodosAsync()
        => (await _service.ObterTodosAsync()).ToHttpResult();

    /// <summary>
    /// Obtém uma lista de estados pelo nome da região - EST02
    /// </summary>
    /// <param name="regiao">Nome da região, exemplo: Norte, Nordeste, Sudeste, Sul e Centro-Oeste.</param>
    /// <response code="200">Retorna a cidade.</response>
    /// <response code="400">Retorna lista de erros, se a requisição for inválida.</response>
    /// <response code="404">Quando nenhum estado é encontrado pelo nome da região fornecida.</response>
    [HttpGet("{regiao}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorIbgeAsync([FromRoute] string regiao)
        => (await _service.ObterTodosPorRegiaoAsync(new ObterTodosPorRegiaoRequest(regiao))).ToHttpResult();
}