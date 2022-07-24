using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Infrastructure.Data.Context;
using SGP.Infrastructure.Data.Repositories.Common;

namespace SGP.Infrastructure.Data.Repositories;

public class EstadoRepository : RepositoryBase<Estado>, IEstadoRepository
{
    public EstadoRepository(SgpContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Estado>> ObterTodosAsync()
        => await DbSet
            .AsNoTrackingWithIdentityResolution()
            .Include(e => e.Regiao)
            .OrderBy(e => e.Nome)
            .ToListAsync();

    public async Task<IEnumerable<Estado>> ObterTodosPorRegiaoAsync(string regiao)
        => await DbSet
            .AsNoTrackingWithIdentityResolution()
            .Include(e => e.Regiao)
            .Where(e => e.Regiao.Nome == regiao)
            .OrderBy(e => e.Nome)
            .ToListAsync();
}