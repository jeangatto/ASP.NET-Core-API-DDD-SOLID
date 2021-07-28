using System.Collections.Generic;
using System.Threading.Tasks;
using FluentResults;
using SGP.Application.Responses;
using SGP.Shared.Interfaces;

namespace SGP.Application.Interfaces
{
    public interface IEstadoService : IAppService
    {
        Task<Result<IEnumerable<EstadoResponse>>> ObterTodosAsync();
    }
}