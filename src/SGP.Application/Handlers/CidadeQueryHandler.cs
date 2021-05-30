using AutoMapper;
using FluentResults;
using Microsoft.Extensions.Caching.Memory;
using SGP.Application.Configuration.Queries;
using SGP.Application.Queries.Cidades;
using SGP.Application.Responses;
using SGP.Domain.Repositories;
using SGP.Shared.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SGP.Application.Handlers
{
    public class CidadeQueryHandler :
        IQueryHandler<ObterCidadesPorUfQuery, Result<IEnumerable<CidadeResponse>>>,
        IQueryHandler<ObterCidadePorIbgeQuery, Result<CidadeResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly ICidadeRepository _repository;

        public CidadeQueryHandler(IMapper mapper, IMemoryCache memoryCache, ICidadeRepository repository)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
            _repository = repository;
        }

        public async Task<Result<IEnumerable<CidadeResponse>>> Handle(ObterCidadesPorUfQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"__{nameof(ObterCidadesPorUfQuery)}__{request}__";

            return await _memoryCache.GetOrCreateAsync(cacheKey, async cacheEntry =>
            {
                // Configuração do cache.
                cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(60);
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2);

                // Obtendo as cidades pelo UF.
                var cidades = await _repository.ObterTodosPorUfAsync(request.Uf);
                if (!cidades.Any())
                {
                    // Retornando não encontrado.
                    return Result.Fail<IEnumerable<CidadeResponse>>(
                        new NotFoundError($"Nenhuma cidade encontrada pelo UF: '{request.Uf}'"));
                }

                // Mapeando domínio para resposta (DTO).
                return Result.Ok(_mapper.Map<IEnumerable<CidadeResponse>>(cidades));
            });
        }

        public async Task<Result<CidadeResponse>> Handle(ObterCidadePorIbgeQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"__{nameof(ObterCidadePorIbgeQuery)}__{request}__";

            return await _memoryCache.GetOrCreateAsync(cacheKey, async cacheEntry =>
            {
                // Configuração do cache.
                cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(60);
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2);

                // Obtendo as cidades pelo UF.
                var cidade = await _repository.ObterPorIbgeAsync(request.Ibge);
                if (cidade == null)
                {
                    // Retornando não encontrado.
                    return Result.Fail<CidadeResponse>(
                        new NotFoundError($"Nenhuma cidade encontrada pelo IBGE: '{request.Ibge}'"));
                }

                // Mapeando domínio para resposta (DTO).
                return Result.Ok(_mapper.Map<CidadeResponse>(cidade));
            });
        }
    }
}
