using System.Security.Claims;
using SGP.Shared.Records;

namespace SGP.Shared.Interfaces;

public interface ITokenClaimsService
{
    AccessToken GenerateAccessToken(Claim[] claims);
    string GenerateRefreshToken();
}