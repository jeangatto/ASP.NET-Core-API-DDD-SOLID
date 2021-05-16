using AutoMapper;
using FluentResults;
using Microsoft.Extensions.Caching.Memory;
using SGP.Application.Interfaces;
using SGP.Application.Requests.CityRequests;
using SGP.Application.Responses;
using SGP.Domain.Repositories;
using SGP.Shared.Errors;
using SGP.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGP.Application.Services
{
    public class CityService : ICityService
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly ICityRepository _repository;

        public CityService(IMapper mapper, IMemoryCache memoryCache, ICityRepository repository)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
            _repository = repository;
        }

        public async Task<Result<IEnumerable<CityResponse>>> GetAllCitiesAsync(GetAllByStateRequest request)
        {
            var cacheKey = $"__CityService__GetAllCitiesAsync__{request}__";

            return await _memoryCache.GetOrCreateAsync(cacheKey, async cacheEntry =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(60);
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2);

                // Validando a requisição.
                var result = await new GetAllByStateRequestValidator().ValidateAsync(request);
                if (!result.IsValid)
                {
                    // Retornando os erros da validação.
                    return result.ToFail<IEnumerable<CityResponse>>();
                }

                // Obtendo as cidades pelo UF.
                var cities = await _repository.GetAllCitiesAsync(request.StateAbbr);
                if (!cities.Any())
                {
                    // Retornando não encontrado.
                    return Result.Fail<IEnumerable<CityResponse>>(
                        new NotFoundError($"Nenhuma cidade encontrada pelo UF: '{request.StateAbbr}'"));
                }

                // Mapeando domínio para resposta (DTO).
                return Result.Ok(_mapper.Map<IEnumerable<CityResponse>>(cities));
            });
        }

        public Task<IEnumerable<string>> GetAllStatesAsync()
        {
            const string cacheKey = "__CityService__GetAllStatesAsync__";

            return _memoryCache.GetOrCreateAsync(cacheKey, cacheEntry =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(60);
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2);
                return _repository.GetAllStatesAsync();
            });
        }

        public async Task<Result<CityResponse>> GetByIbgeAsync(GetByIbgeRequest request)
        {
            var cacheKey = $"__CityService__GetByIbgeAsync__{request}__";

            return await _memoryCache.GetOrCreateAsync(cacheKey, async cacheEntry =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(60);
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2);

                // Validando a requisição.
                var result = await new GetByIbgeRequestValidator().ValidateAsync(request);
                if (!result.IsValid)
                {
                    // Retornando os erros da validação.
                    return result.ToFail<CityResponse>();
                }

                // Obtendo a cidade pelo IBGE.
                var city = await _repository.GetByIbgeAsync(request.Ibge);
                if (city == null)
                {
                    // Retornando não encontrado.
                    return Result.Fail<CityResponse>(
                        new NotFoundError($"Nenhuma cidade encontrada pelo IBGE: '{request.Ibge}'"));
                }

                // Mapeando domínio para resposta (DTO).
                return Result.Ok(_mapper.Map<CityResponse>(city));
            });
        }
    }
}