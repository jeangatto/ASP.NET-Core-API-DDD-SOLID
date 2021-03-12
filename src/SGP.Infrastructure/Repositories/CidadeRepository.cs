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

        public async Task<IEnumerable<Cidade>> GetAllAsync(string estado)
        {
            return await _context.Cidades
                .AsNoTracking()
                .Where(cidade => cidade.Estado == estado)
                .OrderBy(cidade => cidade.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetAllEstadosAsync()
        {
            return await _context.Cidades
                .AsNoTracking()
                .GroupBy(cidade => cidade.Estado)
                .Select(grouping => grouping.Key)
                .OrderBy(estado => estado)
                .ToListAsync();
        }

        public Task<Cidade> GetByIbgeAsync(string ibge)
        {
            return _context.Cidades
                .AsNoTracking()
                .FirstOrDefaultAsync(cidade => cidade.Ibge == ibge);
        }
    }
}
