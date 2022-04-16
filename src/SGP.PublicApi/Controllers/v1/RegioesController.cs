using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.Application.Interfaces;
using SGP.Application.Responses;
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
    /// Obtém uma lista com todas as regiões.
    /// </summary>
    /// <response code="200">Retorna a lista de regiões.</response>
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<RegiaoResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterTodosAsync()
        => (await _service.ObterTodosAsync()).ToHttpResult();
}