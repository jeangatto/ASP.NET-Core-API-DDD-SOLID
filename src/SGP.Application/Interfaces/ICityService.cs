using FluentResults;
using SGP.Application.Requests.CityRequests;
using SGP.Application.Responses;
using SGP.Shared.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGP.Application.Interfaces
{
    public interface ICityService : IService
    {
        Task<Result<IEnumerable<CityResponse>>> GetAllCitiesAsync(GetAllByStateRequest request);
        Task<IEnumerable<string>> GetAllStatesAsync();
        Task<Result<CityResponse>> GetByIbgeAsync(GetByIbgeRequest request);
    }
}