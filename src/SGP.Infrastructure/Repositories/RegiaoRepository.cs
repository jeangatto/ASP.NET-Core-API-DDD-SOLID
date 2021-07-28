using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Infrastructure.Context;

namespace SGP.Infrastructure.Repositories
{
    public class RegiaoRepository : IRegiaoRepository
    {
        private readonly DbSet<Regiao> _dbSet;

        public RegiaoRepository(SgpContext context)
        {
            _dbSet = context.Regioes;
        }

        public async Task<IEnumerable<Regiao>> ObterTodosAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .OrderBy(regiao => regiao.Nome)
                .ToListAsync();
        }
    }
}