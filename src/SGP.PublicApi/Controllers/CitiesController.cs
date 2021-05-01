using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.Application.Interfaces;
using SGP.Application.Requests.CityRequests;
using SGP.Application.Responses;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace SGP.PublicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ICityAppService _service;

        public CitiesController(ICityAppService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtém a lista de municípios do brasil a partir da UF informada.
        /// </summary>
        /// <param name="stateAbbr">Sigla da unidade federativa.</param>
        /// <response code="200">Retorna a lista de municípios.</response>
        /// <response code="400">Retorna lista de erross, se a requisição for inválida.</response>
        /// <returns>Retorna a lista de municípios.</returns>
        [HttpGet("{uf}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<CityResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllCities([FromRoute] string stateAbbr)
        {
            var result = await _service.GetAllCitiesAsync(new GetAllByStateAbbrRequest(stateAbbr));
            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
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