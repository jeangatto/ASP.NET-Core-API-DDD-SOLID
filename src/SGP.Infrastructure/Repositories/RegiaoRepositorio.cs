using Microsoft.EntityFrameworkCore;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGP.Infrastructure.Repositories
{
    public class RegiaoRepositorio : IRegiaoRepositorio
    {
        private readonly DbSet<Regiao> _dbSet;

        public RegiaoRepositorio(SgpContext context) => _dbSet = context.Regioes;

        public async Task<IEnumerable<Regiao>> ObterTodosAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .OrderBy(regiao => regiao.Nome)
                .ToListAsync();
        }
    }
}