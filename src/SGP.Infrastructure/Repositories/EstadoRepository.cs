using Microsoft.EntityFrameworkCore;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGP.Infrastructure.Repositories
{
    public class EstadoRepository : IEstadoRepository
    {
        private readonly DbSet<Estado> _dbSet;

        public EstadoRepository(SgpContext context)
        {
            _dbSet = context.Estados;
        }

        public async Task<IEnumerable<Estado>> ObterTodosAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .Include(estado => estado.Regiao)
                .OrderBy(estado => estado.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<Estado>> ObterTodosPorRegiaoAsync(string regiao)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(estado => estado.Regiao)
                .Where(estado => estado.Regiao.Nome == regiao)
                .OrderBy(estado => estado.Nome)
                .ToListAsync();
        }
    }
}
