using FluentResults;
using SGP.Application.Requests;
using SGP.Application.Requests.UsuarioRequests;
using SGP.Application.Responses;
using SGP.Shared.Interfaces;
using System.Threading.Tasks;

namespace SGP.Application.Interfaces
{
    public interface IUsuarioAppService : IAppService
    {
        Task<Result<CreatedResponse>> AddAsync(AddUsuarioRequest request);
        Task<Result<UsuarioResponse>> GetByIdAsync(GetByIdRequest request);
    }
}
