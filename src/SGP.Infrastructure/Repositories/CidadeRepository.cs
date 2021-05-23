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
        private readonly DbSet<Cidade> _dbSet;

        public CidadeRepository(SgpContext context) => _dbSet = context.Cidades;

        public async Task<Cidade> ObterPorIbgeAsync(int ibge)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(cidade => cidade.Estado)
                    .ThenInclude(estado => estado.Regiao)
                .FirstOrDefaultAsync(cidade => cidade.Ibge == ibge);
        }

        public async Task<IEnumerable<Cidade>> ObterTodosPorUfAsync(string uf)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(cidade => cidade.Estado)
                    .ThenInclude(estado => estado.Regiao)
                .Where(cidade => cidade.Estado.Uf == uf)
                .OrderBy(cidade => cidade.Nome)
                    .ThenBy(cidade => cidade.Ibge)
                .ToListAsync();
        }
    }
}
