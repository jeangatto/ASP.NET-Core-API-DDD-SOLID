using System.Collections.Generic;
using System.Threading.Tasks;
using SGP.Domain.Entities;
using SGP.Shared.Abstractions;

namespace SGP.Domain.Repositories;

public interface IEstadoRepository : IRepository
{
    Task<IEnumerable<Estado>> ObterTodosAsync();
    Task<IEnumerable<Estado>> ObterTodosPorRegiaoAsync(string regiao);
}
