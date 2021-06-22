using SGP.Application.Responses;
using SGP.Shared.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGP.Application.Interfaces
{
    public interface IEstadoService : IAppService
    {
        Task<IEnumerable<EstadoResponse>> ObterTodosAsync();
    }
}