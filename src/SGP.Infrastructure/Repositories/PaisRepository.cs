using Microsoft.EntityFrameworkCore;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGP.Infrastructure.Repositories
{
    public class PaisRepository : IPaisRepository
    {
        private readonly SGPContext _context;

        public PaisRepository(SGPContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pais>> GetAllAsync()
        {
            return await _context.Paises
                .AsNoTracking()
                .OrderBy(p => p.Nome)
                .ThenBy(p => p.Sigla)
                .ToListAsync();
        }
    }
}