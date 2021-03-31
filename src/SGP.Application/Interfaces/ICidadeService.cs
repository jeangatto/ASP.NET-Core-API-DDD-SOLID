using SGP.Application.Requests.CidadeRequests;
using SGP.Application.Responses;
using SGP.Shared.Interfaces;
using SGP.Shared.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGP.Application.Interfaces
{
    public interface ICidadeService : IService
    {
        Task<IResult<IEnumerable<CidadeResponse>>> GetAllAsync(GetAllByEstadoRequest request);
        Task<IEnumerable<string>> GetAllEstadosAsync();
        Task<IResult<CidadeResponse>> GetByIbgeAsync(GetByIbgeRequest request);
    }
}
