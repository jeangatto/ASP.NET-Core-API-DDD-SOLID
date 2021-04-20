using FluentResults;
using SGP.Application.Requests.CidadeRequests;
using SGP.Application.Responses;
using SGP.Shared.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGP.Application.Interfaces
{
    public interface ICidadeAppService : IAppService
    {
        Task<Result<IEnumerable<CidadeResponse>>> GetAllAsync(GetAllByEstadoRequest request);
        Task<IEnumerable<string>> GetAllEstadosAsync();
        Task<Result<CidadeResponse>> GetByIbgeAsync(GetByIbgeRequest request);
    }
}
