using System.Security.Claims;
using SGP.Shared.Records;

namespace SGP.Shared.Abstractions;

public interface ITokenClaimsService
{
    AccessToken GenerateAccessToken(Claim[] claims);
    string GenerateRefreshToken();
}