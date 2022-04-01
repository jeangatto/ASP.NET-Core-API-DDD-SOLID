using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.Application.Interfaces;
using SGP.Application.Requests.CidadeRequests;
using SGP.PublicApi.Extensions;

namespace SGP.PublicApi.Controllers.v1;

[Route("api/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class CidadesController : ControllerBase
{
    private readonly ICidadeService _service;

    public CidadesController(ICidadeService service) => _service = service;

    /// <summary>
    /// Listar as cidades por UF - CID01
    /// </summary>
    /// <param name="uf">Sigla da unidade federativa (UF).</param>
    /// <response code="200">Retorna a lista de cidades.</response>
    /// <response code="400">Retorna lista de erros, se a requisição for inválida.</response>
    /// <response code="404">Quando nenhuma cidade é encontrada pelo UF fornecido.</response>
    [HttpGet("{uf:alpha}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterTodosPorUfAsync([FromRoute] string uf)
        => (await _service.ObterTodosPorUfAsync(new ObterTodosPorUfRequest(uf))).ToHttpResult();

    /// <summary>
    /// Obter a cidade pelo código de IBGE - CID02
    /// </summary>
    /// <param name="ibge">Código de IBGE.</param>
    /// <response code="200">Retorna a cidade.</response>
    /// <response code="400">Retorna lista de erros, se a requisição for inválida.</response>
    /// <response code="404">Quando nenhuma cidade é encontrada pelo IBGE fornecido.</response>
    [HttpGet("{ibge:int}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorIbgeAsync([FromRoute] int ibge)
        => (await _service.ObterPorIbgeAsync(new ObterPorIbgeRequest(ibge))).ToHttpResult();
}