using System.Threading.Tasks;
using FluentResults;
using SGP.Application.Requests.AuthenticationRequests;
using SGP.Application.Responses;
using SGP.Shared.Interfaces;

namespace SGP.Application.Interfaces;

public interface IAuthenticationService : IAppService
{
    Task<Result<TokenResponse>> AuthenticateAsync(LogInRequest request);
    Task<Result<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request);
}