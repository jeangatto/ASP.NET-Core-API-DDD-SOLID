using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.Application.Interfaces;
using SGP.Application.Requests.CidadeRequests;
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

        [HttpGet("{uf}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
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