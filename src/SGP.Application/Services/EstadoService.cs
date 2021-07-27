namespace SGP.Application.Services
{
    using AutoMapper;
    using Common;
    using Domain.Repositories;
    using FluentResults;
    using Interfaces;
    using Microsoft.Extensions.Caching.Memory;
    using Responses;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class EstadoService : BaseService, IEstadoService
    {
        private const string ObterTodosCacheKey = "EstadoService__ObterTodosAsync";
        private readonly IMapper _mapper;
        private readonly IEstadoRepository _repository;

        public EstadoService(IMapper mapper, IMemoryCache memoryCache, IEstadoRepository repository)
            : base(memoryCache)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Result<IEnumerable<EstadoResponse>>> ObterTodosAsync()
        {
            return await MemoryCache.GetOrCreateAsync(ObterTodosCacheKey, async cacheEntry =>
            {
                ConfigureCacheEntry(cacheEntry);
                var estados = await _repository.ObterTodosAsync();
                return Result.Ok(_mapper.Map<IEnumerable<EstadoResponse>>(estados));
            });
        }
    }
}
