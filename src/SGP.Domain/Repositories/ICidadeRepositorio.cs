using SGP.Domain.Entities;
using SGP.Shared.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGP.Domain.Repositories
{
    public interface ICidadeRepositorio : IRepository
    {
        Task<IEnumerable<Cidade>> ObterTodosPorUfAsync(string uf);
        Task<Cidade> ObterPorIbgeAsync(int ibge);
    }
}
