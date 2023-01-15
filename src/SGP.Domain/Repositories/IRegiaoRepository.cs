using System.Collections.Generic;
using System.Threading.Tasks;
using SGP.Domain.Entities;
using SGP.Shared.Abstractions;

namespace SGP.Domain.Repositories;

public interface IRegiaoRepository : IRepository
{
    Task<IEnumerable<Regiao>> ObterTodosAsync();
}