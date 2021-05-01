using Microsoft.EntityFrameworkCore;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGP.Infrastructure.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly SgpContext _context;

        public CityRepository(SgpContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<City>> GetAllCitiesAsync(string stateAbbr)
        {
            return await _context.Cities
                .AsNoTracking()
                .Where(city => city.StateAbbr == stateAbbr)
                .OrderBy(city => city.Name)
                .ThenBy(city => city.Ibge)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetAllStatesAsync()
        {
            return await _context.Cities
                .AsNoTracking()
                .GroupBy(city => city.StateAbbr)
                .Select(grouping => grouping.Key)
                .OrderBy(stateAbbr => stateAbbr)
                .ToListAsync();
        }

        public Task<City> GetByIbgeAsync(string ibge)
        {
            return _context.Cities
                .AsNoTracking()
                .FirstOrDefaultAsync(city => city.Ibge == ibge);
        }
    }
}
