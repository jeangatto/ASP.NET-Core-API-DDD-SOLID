using FluentResults;
using SGP.Application.Requests;
using SGP.Application.Requests.UserRequests;
using SGP.Application.Responses;
using SGP.Shared.Interfaces;
using System.Threading.Tasks;

namespace SGP.Application.Interfaces
{
    public interface IUserService : IService
    {
        Task<Result<UserResponse>> CreateAsync(CreateUserRequest request);
        Task<Result<UserResponse>> GetByIdAsync(GetByIdRequest request);
    }
}
