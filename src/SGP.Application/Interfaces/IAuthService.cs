using System.Threading.Tasks;
using FluentResults;
using SGP.Application.Requests.AuthRequests;
using SGP.Application.Responses;
using SGP.Shared.Interfaces;

namespace SGP.Application.Interfaces
{
    public interface IAuthService : IAppService
    {
        Task<Result<TokenResponse>> AuthenticateAsync(AuthRequest request);
        Task<Result<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request);
    }
}