using SGP.Domain.Entities;
using SGP.Shared.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGP.Domain.Repositories
{
    public interface IEstadoRepository : IRepository
    {
        Task<IEnumerable<Estado>> GetAllAsync();
    }
}