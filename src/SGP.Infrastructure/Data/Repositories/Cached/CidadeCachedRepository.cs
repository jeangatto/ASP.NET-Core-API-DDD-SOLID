using System.Collections.Generic;
using System.Threading.Tasks;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Infrastructure.Data.Repositories.Common;
using SGP.Shared.Interfaces;

namespace SGP.Infrastructure.Data.Repositories.Cached;

public class CidadeCachedRepository : CachedRepositoryBase<ICidadeRepository>, ICidadeRepository
{
    private const string RootName = nameof(ICidadeRepository);
    private const string ObterPorIbgeCacheKey = $"{RootName}__{nameof(ObterPorIbgeAsync)}__{{0}}";
    private const string ObterTodosPorUfCacheKey = $"{RootName}__{nameof(ObterTodosPorUfAsync)}__{{0}}";

    public CidadeCachedRepository(ICacheService cacheService, ICidadeRepository repository)
        : base(cacheService, repository) { }

    public async Task<Cidade> ObterPorIbgeAsync(int ibge)
    {
        var cacheKey = string.Format(ObterPorIbgeCacheKey, ibge);
        return await CacheService.GetOrCreateAsync(cacheKey, () => Repository.ObterPorIbgeAsync(ibge));
    }

    public async Task<IEnumerable<Cidade>> ObterTodosPorUfAsync(string uf)
    {
        var cacheKey = string.Format(ObterTodosPorUfCacheKey, uf);
        return await CacheService.GetOrCreateAsync(cacheKey, () => Repository.ObterTodosPorUfAsync(uf));
    }
}