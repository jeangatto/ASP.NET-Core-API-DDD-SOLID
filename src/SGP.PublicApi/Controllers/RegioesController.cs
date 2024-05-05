using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.Application.Interfaces;
using SGP.Application.Responses;
using SGP.PublicApi.Extensions;
using SGP.PublicApi.Models;

namespace SGP.PublicApi.Controllers;

[Route("api/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class RegioesController(IRegiaoService service) : ControllerBase
{
    /// <summary>
    /// Obtém uma lista com todas as regiões.
    /// </summary>
    /// <response code="200">Retorna a lista de regiões.</response>
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<RegiaoResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterTodosAsync() =>
        (await service.ObterTodosAsync()).ToActionResult();
}