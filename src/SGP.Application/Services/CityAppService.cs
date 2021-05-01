using AutoMapper;
using FluentResults;
using SGP.Application.Interfaces;
using SGP.Application.Requests.CityRequests;
using SGP.Application.Responses;
using SGP.Domain.Repositories;
using SGP.Shared.Extensions;
using System.Collections.Generic;
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
            var result = await new GetAllByStateAbbrRequestValidator().ValidateAsync(request);
            if (!result.IsValid)
            {
                return result.ToFail<IEnumerable<CityResponse>>();
            }

            var cities = await _repository.GetAllCitiesAsync(request.StateAbbr);
            return Result.Ok(_mapper.Map<IEnumerable<CityResponse>>(cities));
        }

        public async Task<IEnumerable<string>> GetAllStatesAsync()
        {
            return await _repository.GetAllStatesAsync();
        }

        public async Task<Result<CityResponse>> GetByIbgeAsync(GetByIbgeRequest request)
        {
            var result = await new GetByIbgeRequestValidator().ValidateAsync(request);
            if (!result.IsValid)
            {
                return result.ToFail<CityResponse>();
            }

            var city = await _repository.GetByIbgeAsync(request.Ibge);
            if (city == null)
            {
                return Result.Fail<CityResponse>($"Nenhuma cidade encontrada pelo IBGE: '{request.Ibge}'");
            }

            return Result.Ok(_mapper.Map<CityResponse>(city));
        }
    }
}