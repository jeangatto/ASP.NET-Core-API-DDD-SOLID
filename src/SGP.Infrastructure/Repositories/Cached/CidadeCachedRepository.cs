using System.Collections.Generic;
using System.Threading.Tasks;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Shared.Interfaces;

namespace SGP.Infrastructure.Repositories.Cached;

public class CidadeCachedRepository : ICidadeRepository
{
    private const string RootName = nameof(ICidadeRepository);
    private const string ObterPorIbgeCacheKey = $"{RootName}__{nameof(ObterPorIbgeAsync)}__{{0}}";
    private const string ObterTodosPorUfCacheKey = $"{RootName}__{nameof(ObterTodosPorUfAsync)}__{{0}}";

    private readonly ICacheService _cacheService;
    private readonly ICidadeRepository _repository;

    public CidadeCachedRepository(ICacheService cacheService, ICidadeRepository repository)
    {
        _cacheService = cacheService;
        _repository = repository;
    }

    public async Task<Cidade> ObterPorIbgeAsync(int ibge)
    {
        var cacheKey = string.Format(ObterPorIbgeCacheKey, ibge);
        return await _cacheService.GetOrCreateAsync(cacheKey, () => _repository.ObterPorIbgeAsync(ibge));
    }

    public async Task<IEnumerable<Cidade>> ObterTodosPorUfAsync(string uf)
    {
        var cacheKey = string.Format(ObterTodosPorUfCacheKey, uf);
        return await _cacheService.GetOrCreateAsync(cacheKey, () => _repository.ObterTodosPorUfAsync(uf));
    }
}