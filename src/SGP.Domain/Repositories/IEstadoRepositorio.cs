using SGP.Domain.Entities;
using SGP.Shared.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGP.Domain.Repositories
{
    public interface IEstadoRepositorio : IRepository
    {
        Task<IEnumerable<Estado>> ObterTodosAsync();
        Task<IEnumerable<Estado>> ObterTodosPorRegiaoAsync(string regiao);
    }
}
