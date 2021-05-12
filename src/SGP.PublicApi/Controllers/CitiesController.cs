using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.Application.Interfaces;
using SGP.Application.Requests.CityRequests;
using SGP.Application.Responses;
using SGP.Shared.Errors;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace SGP.PublicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ICityService _service;

        public CitiesController(ICityService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtém a lista de municípios do brasil a partir da sigla do estado (UF) fornecido.
        /// </summary>
        /// <param name="state">Sigla UF (Unidade Federativa).</param>
        /// <response code="200">Retorna a lista de municípios.</response>
        /// <response code="400">Retorna lista de erros, se a requisição for inválida.</response>
        /// <response code="404">Quando nenhuma cidade é encontrada pelo UF fornecido.</response>
        /// <returns>Retorna a lista de municípios.</returns>
        [HttpGet("{state}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<CityResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllCities([FromRoute] string state)
        {
            var request = new GetAllByStateRequest(state);

            var result = await _service.GetAllCitiesAsync(request);
            if (result.IsFailed)
            {
                return result.HasError<NotFoundError>() ?
                    NotFound(result.Errors) : BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Obtém a lista dos estados do brasil.
        /// </summary>
        /// <response code="200">Retorna a lista de estados.</response>
        /// <returns>Retorna a lista de estados.</returns>
        [HttpGet("states")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllStates()
        {
            return Ok(await _service.GetAllStatesAsync());
        }
    }
}