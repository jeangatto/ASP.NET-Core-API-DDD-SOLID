using SGP.Domain.Entities;
using SGP.Shared.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGP.Domain.Repositories
{
    public interface IPaisRepository : IRepository
    {
        Task<IEnumerable<Pais>> GetAllAsync();
    }
}