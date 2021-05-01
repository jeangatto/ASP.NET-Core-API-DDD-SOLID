using SGP.Domain.Entities;
using SGP.Shared.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGP.Domain.Repositories
{
    public interface ICityRepository : IRepository
    {
        Task<IEnumerable<City>> GetAllCitiesAsync(string stateAbbr);
        Task<IEnumerable<string>> GetAllStatesAsync();
        Task<City> GetByIbgeAsync(string ibge);
    }
}