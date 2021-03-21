using SGP.Application.Requests.AuthRequests;
using SGP.Application.Responses;
using SGP.Shared.Results;
using System.Threading.Tasks;

namespace SGP.Application.Interfaces
{
    public interface IAuthService
    {
        Task<IResult<TokenResponse>> AuthenticateAsync(AuthRequest request);
        Task<IResult<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request);
    }
}