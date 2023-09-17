using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.Result;
using SGP.Application.Requests.EstadoRequests;
using SGP.Application.Responses;
using SGP.Shared.Abstractions;

namespace SGP.Application.Interfaces;

public interface IEstadoService : IAppService
{
    Task<Result<IEnumerable<EstadoResponse>>> ObterTodosAsync();
    Task<Result<IEnumerable<EstadoResponse>>> ObterTodosPorRegiaoAsync(ObterTodosPorRegiaoRequest request);
}
