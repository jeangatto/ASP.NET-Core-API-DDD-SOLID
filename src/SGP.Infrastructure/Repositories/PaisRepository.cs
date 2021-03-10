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
        private readonly SgpContext _context;

        public PaisRepository(SgpContext context)
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