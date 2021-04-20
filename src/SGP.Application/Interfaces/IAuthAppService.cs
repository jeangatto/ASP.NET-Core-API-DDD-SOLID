using FluentResults;
using SGP.Application.Requests.AuthRequests;
using SGP.Application.Responses;
using SGP.Shared.Interfaces;
using System.Threading.Tasks;

namespace SGP.Application.Interfaces
{
    public interface IAuthAppService : IAppService
    {
        Task<Result<TokenResponse>> AuthenticateAsync(AuthRequest request);
        Task<Result<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request);
    }
}