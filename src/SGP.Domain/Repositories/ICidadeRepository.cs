using SGP.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGP.Domain.Repositories
{
    public interface ICidadeRepository
    {
        Task<IEnumerable<Cidade>> GetAllAsync(string estado);
        Task<IEnumerable<string>> GetAllEstadosAsync();
        Task<Cidade> GetByIbgeAsync(string ibge);
    }
}