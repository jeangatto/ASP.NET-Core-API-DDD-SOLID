using System.Collections.Generic;
using System.Threading.Tasks;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Infrastructure.Data.Repositories.Common;
using SGP.Shared.Abstractions;

namespace SGP.Infrastructure.Data.Repositories.Cached;

public class CidadeCachedRepository(ICacheService cacheService, ICidadeRepository repository)
    : CachedRepositoryBase<ICidadeRepository>(cacheService, repository), ICidadeRepository
{
    private const string RootName = nameof(ICidadeRepository);
    private const string ObterPorIbgeCacheKey = $"{RootName}__{nameof(ObterPorIbgeAsync)}__{{0}}";
    private const string ObterTodosPorUfCacheKey = $"{RootName}__{nameof(ObterTodosPorUfAsync)}__{{0}}";

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