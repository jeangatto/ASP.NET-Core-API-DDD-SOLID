using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.Application.Interfaces;
using SGP.PublicApi.Extensions;

namespace SGP.PublicApi.Controllers.v1;

[Route("api/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class RegioesController : ControllerBase
{
    private readonly IRegiaoService _service;

    public RegioesController(IRegiaoService service) => _service = service;

    /// <summary>
    /// Obtém uma lista com todas as regiões - REG01
    /// </summary>
    /// <response code="200">Retorna a lista de regiões.</response>
    /// <response code="400">Retorna lista de erros, se a requisição for inválida.</response>
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ObterTodosAsync()
        => (await _service.ObterTodosAsync()).ToHttpResult();
}