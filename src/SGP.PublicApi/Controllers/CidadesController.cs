using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.Application.Interfaces;
using SGP.Application.Requests.CidadeRequests;
using SGP.Application.Responses;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace SGP.PublicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CidadesController : ControllerBase
    {
        private readonly ICidadeService _service;

        public CidadesController(ICidadeService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtém a lista de municípios do brasil a partir da UF informada.
        /// </summary>
        /// <param name="uf">Sigla da unidade federativa.</param>
        /// <response code="200">Retorna a lista de municípios.</response>
        /// <response code="400">Retorna lista de erross, se a requisição for inválida.</response>
        /// <returns>Retorna a lista de municípios.</returns>
        [HttpGet("{uf}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<CidadeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Listar([FromRoute] string uf)
        {
            var result = await _service.GetAllAsync(new GetAllByEstadoRequest(uf));
            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }
    }
}