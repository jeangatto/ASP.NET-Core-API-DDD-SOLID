using Microsoft.EntityFrameworkCore;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGP.Infrastructure.Repositories
{
    public class CidadeRepository : ICidadeRepository
    {
        private readonly SgpContext _context;

        public CidadeRepository(SgpContext context)
        {
            _context = context;
        }

        public Task<Cidade> GetByIbgeAsync(int estadoIbge)
        {
            return _context.Cidades
                .AsNoTracking()
                .Include(c => c.Estado)
                .ThenInclude(e => e.Pais)
                .FirstOrDefaultAsync(c => c.Estado.Ibge == estadoIbge);
        }

        public async Task<IEnumerable<Cidade>> GetByUfAsync(string estadoSigla)
        {
            return await _context.Cidades
                .AsNoTracking()
                .Include(c => c.Estado)
                .ThenInclude(e => e.Pais)
                .Where(c => c.Estado.Sigla == estadoSigla)
                .OrderBy(c => c.Nome)
                .ToListAsync();
        }
    }
}