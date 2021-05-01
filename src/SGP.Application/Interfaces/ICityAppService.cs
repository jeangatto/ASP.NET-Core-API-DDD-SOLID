using FluentResults;
using SGP.Application.Requests.CityRequests;
using SGP.Application.Responses;
using SGP.Shared.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGP.Application.Interfaces
{
    public interface ICityAppService : IAppService
    {
        Task<Result<IEnumerable<CityResponse>>> GetAllCitiesAsync(GetAllByStateAbbrRequest request);
        Task<IEnumerable<string>> GetAllStatesAsync();
        Task<Result<CityResponse>> GetByIbgeAsync(GetByIbgeRequest request);
    }
}