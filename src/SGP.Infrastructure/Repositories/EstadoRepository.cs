using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Infrastructure.Context;

namespace SGP.Infrastructure.Repositories;

public class EstadoRepository : IEstadoRepository
{
    private readonly DbSet<Estado> _dbSet;

    public EstadoRepository(SgpContext context)
        => _dbSet = context.Estados;

    public async Task<IEnumerable<Estado>> ObterTodosAsync()
        => await _dbSet
            .AsNoTrackingWithIdentityResolution()
            .Include(e => e.Regiao)
            .OrderBy(e => e.Nome)
            .ToListAsync();

    public async Task<IEnumerable<Estado>> ObterTodosPorRegiaoAsync(string regiao)
        => await _dbSet
            .AsNoTrackingWithIdentityResolution()
            .Include(e => e.Regiao)
            .Where(e => e.Regiao.Nome == regiao)
            .OrderBy(e => e.Nome)
            .ToListAsync();
}