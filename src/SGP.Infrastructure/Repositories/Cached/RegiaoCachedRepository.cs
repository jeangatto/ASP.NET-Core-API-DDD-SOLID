using System.Collections.Generic;
using System.Threading.Tasks;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Shared.Interfaces;

namespace SGP.Infrastructure.Repositories.Cached;

public class RegiaoCachedRepository : CachedRepositoryBase<IRegiaoRepository>, IRegiaoRepository
{
    private const string RootName = nameof(IRegiaoRepository);
    private const string ObterTodosCacheKey = $"{RootName}__{nameof(ObterTodosAsync)}";

    public RegiaoCachedRepository(ICacheService cacheService, IRegiaoRepository repository)
        : base(cacheService, repository) { }

    public async Task<IEnumerable<Regiao>> ObterTodosAsync()
        => await CacheService.GetOrCreateAsync(ObterTodosCacheKey, () => Repository.ObterTodosAsync());
}