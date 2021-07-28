using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentResults;
using Microsoft.Extensions.Caching.Memory;
using SGP.Application.Interfaces;
using SGP.Application.Requests.CidadeRequests;
using SGP.Application.Responses;
using SGP.Application.Services.Common;
using SGP.Domain.Repositories;
using SGP.Shared.Errors;
using SGP.Shared.Extensions;

namespace SGP.Application.Services
{
    public class CidadeService : BaseService, ICidadeService
    {
        private const string ObterPorIbgeCacheKey = "CidadeService__ObterPorIbgeAsync__{0}";
        private const string ObterTodosPorUfCacheKey = "CidadeService__ObterTodosPorUfAsync__{0}";
        private readonly IMapper _mapper;
        private readonly ICidadeRepository _repository;

        public CidadeService(IMapper mapper, IMemoryCache memoryCache, ICidadeRepository repository)
            : base(memoryCache)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Result<CidadeResponse>> ObterPorIbgeAsync(ObterPorIbgeRequest request)
        {
            var cacheKey = string.Format(ObterPorIbgeCacheKey, request.Ibge);

            return await MemoryCache.GetOrCreateAsync(cacheKey, async cacheEntry =>
            {
                ConfigureCacheEntry(cacheEntry);

                // Validando a requisição.
                request.Validate();
                if (!request.IsValid)
                {
                    // Retornando os erros da validação.
                    return request.ToFail<CidadeResponse>();
                }

                // Obtendo a cidade pelo IBGE.
                var cidade = await _repository.ObterPorIbgeAsync(request.Ibge);
                if (cidade == null)
                {
                    // Retornando não encontrado.
                    return Result.Fail<CidadeResponse>(
                        new NotFoundError($"Nenhuma cidade encontrada pelo IBGE: {request.Ibge}"));
                }

                // Mapeando domínio para resposta (DTO).
                return Result.Ok(_mapper.Map<CidadeResponse>(cidade));
            });
        }

        public async Task<Result<IEnumerable<CidadeResponse>>> ObterTodosPorUfAsync(ObterTodosPorUfRequest request)
        {
            var cacheKey = string.Format(ObterTodosPorUfCacheKey, request.Uf);

            return await MemoryCache.GetOrCreateAsync(cacheKey, async cacheEntry =>
            {
                ConfigureCacheEntry(cacheEntry);

                // Validando a requisição.
                request.Validate();
                if (!request.IsValid)
                {
                    // Retornando os erros da validação.
                    return request.ToFail<IEnumerable<CidadeResponse>>();
                }

                // Obtendo as cidades pelo UF.
                var cidades = await _repository.ObterTodosPorUfAsync(request.Uf);
                if (!cidades.Any())
                {
                    // Retornando não encontrado.
                    return Result.Fail<IEnumerable<CidadeResponse>>(
                        new NotFoundError($"Nenhuma cidade encontrada pelo UF: {request.Uf}"));
                }

                // Mapeando domínio para resposta (DTO).
                return Result.Ok(_mapper.Map<IEnumerable<CidadeResponse>>(cidades));
            });
        }
    }
}