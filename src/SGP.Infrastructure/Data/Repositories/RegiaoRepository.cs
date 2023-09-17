using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Infrastructure.Data.Context;
using SGP.Infrastructure.Data.Repositories.Common;

namespace SGP.Infrastructure.Data.Repositories;

public class RegiaoRepository : RepositoryBase<Regiao>, IRegiaoRepository
{
    public RegiaoRepository(SgpContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Regiao>> ObterTodosAsync() =>
        await DbSet.AsNoTracking().OrderBy(r => r.Nome).ToListAsync();
}
