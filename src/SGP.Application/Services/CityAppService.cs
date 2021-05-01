using AutoMapper;
using FluentResults;
using SGP.Application.Interfaces;
using SGP.Application.Requests.CityRequests;
using SGP.Application.Responses;
using SGP.Domain.Repositories;
using SGP.Shared.Errors;
using SGP.Shared.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGP.Application.Services
{
    public class CityAppService : ICityAppService
    {
        private readonly IMapper _mapper;
        private readonly ICityRepository _repository;

        public CityAppService(IMapper mapper, ICityRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Result<IEnumerable<CityResponse>>> GetAllCitiesAsync(GetAllByStateAbbrRequest request)
        {
            // Validando a requisição.
            var result = await new GetAllByStateAbbrRequestValidator().ValidateAsync(request);
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
        }

        public async Task<IEnumerable<string>> GetAllStatesAsync()
        {
            return await _repository.GetAllStatesAsync();
        }

        public async Task<Result<CityResponse>> GetByIbgeAsync(GetByIbgeRequest request)
        {
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
        }
    }
}