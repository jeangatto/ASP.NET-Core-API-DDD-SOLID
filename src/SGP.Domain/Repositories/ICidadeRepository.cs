using System.Collections.Generic;
using System.Threading.Tasks;
using SGP.Domain.Entities;
using SGP.Shared.Interfaces;

namespace SGP.Domain.Repositories
{
    public interface ICidadeRepository : IRepository
    {
        Task<Cidade> ObterPorIbgeAsync(int ibge);
        Task<IEnumerable<Cidade>> ObterTodosPorUfAsync(string uf);
    }
}