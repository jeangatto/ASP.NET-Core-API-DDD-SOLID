using SGP.Domain.Entities;
using SGP.Shared.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGP.Domain.Repositories
{
    public interface IPaisRepository : IRepository
    {
        Task<IEnumerable<Pais>> GetAllAsync();
    }
}