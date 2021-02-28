using SGP.Domain.Entities;
using SGP.Shared.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGP.Domain.Repositories
{
    public interface ICidadeRepository : IRepository
    {
        Task<Cidade> GetByIbgeAsync(int estadoIbge);
        Task<IEnumerable<Cidade>> GetByUfAsync(string estadoSigla);
    }
}
