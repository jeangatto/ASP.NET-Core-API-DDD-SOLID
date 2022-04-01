using System.Collections.Generic;
using System.Threading.Tasks;
using FluentResults;
using SGP.Application.Requests.CidadeRequests;
using SGP.Application.Responses;
using SGP.Shared.Interfaces;

namespace SGP.Application.Interfaces;

public interface ICidadeService : IAppService
{
    Task<Result<CidadeResponse>> ObterPorIbgeAsync(ObterPorIbgeRequest request);
    Task<Result<IEnumerable<CidadeResponse>>> ObterTodosPorUfAsync(ObterTodosPorUfRequest request);
}