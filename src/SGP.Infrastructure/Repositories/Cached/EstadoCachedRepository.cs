using System.Collections.Generic;
using System.Threading.Tasks;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Shared.Interfaces;

namespace SGP.Infrastructure.Repositories.Cached;

public class EstadoCachedRepository : IEstadoRepository
{
    private const string RootName = nameof(IEstadoRepository);
    private const string ObterTodosCacheKey = $"{RootName}__{nameof(ObterTodosAsync)}";
    private const string ObterTodosPorRegiaoCacheKey = $"{RootName}__{nameof(ObterTodosPorRegiaoAsync)}__{{0}}";

    private readonly ICacheService _cacheService;
    private readonly IEstadoRepository _repository;

    public EstadoCachedRepository(ICacheService cacheService, IEstadoRepository repository)
    {
        _cacheService = cacheService;
        _repository = repository;
    }

    public async Task<IEnumerable<Estado>> ObterTodosAsync()
        => await _cacheService.GetOrCreateAsync(ObterTodosCacheKey, () => _repository.ObterTodosAsync());

    public async Task<IEnumerable<Estado>> ObterTodosPorRegiaoAsync(string regiao)
    {
        var cacheKey = string.Format(ObterTodosPorRegiaoCacheKey, regiao);
        return await _cacheService.GetOrCreateAsync(cacheKey, () => _repository.ObterTodosPorRegiaoAsync(regiao));
    }
}