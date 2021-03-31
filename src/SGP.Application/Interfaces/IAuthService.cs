using SGP.Application.Requests.AuthRequests;
using SGP.Application.Responses;
using SGP.Shared.Interfaces;
using SGP.Shared.Results;
using System.Threading.Tasks;

namespace SGP.Application.Interfaces
{
    public interface IAuthService : IService
    {
        Task<IResult<TokenResponse>> AuthenticateAsync(AuthRequest request);
        Task<IResult<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request);
    }
}