using FluentResults;
using SGP.Application.Requests.CidadeRequests;
using SGP.Application.Responses;
using SGP.Shared.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGP.Application.Interfaces
{
    public interface ICidadeService : IAppService
    {
        Task<Result<CidadeResponse>> ObterPorIbgeAsync(ObterPorIbgeRequest request);
        Task<Result<IEnumerable<CidadeResponse>>> ObterTodosPorUfAsync(ObterTodosPorUfRequest request);
    }
}