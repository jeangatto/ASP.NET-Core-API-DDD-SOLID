using SGP.Application.Requests.CidadeRequests;
using SGP.Application.Responses;
using SGP.Shared.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGP.Application.Interfaces
{
    public interface ICidadeAppService
    {
        Task<IResult<IEnumerable<CidadeResponse>>> GetAllAsync(GetAllByEstadoRequest request);
        Task<IResult<IEnumerable<string>>> GetAllEstadosAsync();
    }
}
