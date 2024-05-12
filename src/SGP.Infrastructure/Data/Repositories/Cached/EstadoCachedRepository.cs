using System.Collections.Generic;
using System.Threading.Tasks;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Infrastructure.Data.Repositories.Common;
using SGP.Shared.Abstractions;

namespace SGP.Infrastructure.Data.Repositories.Cached;

public class EstadoCachedRepository(ICacheService cacheService, IEstadoRepository repository)
    : CachedRepositoryBase<IEstadoRepository>(cacheService, repository), IEstadoRepository
{
    private const string RootName = nameof(IEstadoRepository);
    private const string ObterTodosCacheKey = $"{RootName}__{nameof(ObterTodosAsync)}";
    private const string ObterTodosPorRegiaoCacheKey = $"{RootName}__{nameof(ObterTodosPorRegiaoAsync)}__{{0}}";

    public async Task<IEnumerable<Estado>> ObterTodosAsync() =>
        await CacheService.GetOrCreateAsync(ObterTodosCacheKey, () => Repository.ObterTodosAsync());

    public async Task<IEnumerable<Estado>> ObterTodosPorRegiaoAsync(string regiao)
    {
        var cacheKey = string.Format(ObterTodosPorRegiaoCacheKey, regiao);
        return await CacheService.GetOrCreateAsync(cacheKey, () => Repository.ObterTodosPorRegiaoAsync(regiao));
    }
}