using System.Collections.Generic;
using System.Threading.Tasks;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Shared.Interfaces;

namespace SGP.Infrastructure.Repositories.Cached;

public class RegiaoCachedRepository : IRegiaoRepository
{
    private const string RootName = nameof(IRegiaoRepository);
    private const string ObterTodosCacheKey = $"{RootName}__{nameof(ObterTodosAsync)}";

    private readonly ICacheService _cacheService;
    private readonly IRegiaoRepository _repository;

    public RegiaoCachedRepository(ICacheService cacheService, IRegiaoRepository repository)
    {
        _cacheService = cacheService;
        _repository = repository;
    }

    public async Task<IEnumerable<Regiao>> ObterTodosAsync()
        => await _cacheService.GetOrCreateAsync(ObterTodosCacheKey, () => _repository.ObterTodosAsync());
}