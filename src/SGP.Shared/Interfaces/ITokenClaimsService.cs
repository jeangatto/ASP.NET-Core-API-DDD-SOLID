using System.Collections.Generic;
using System.Security.Claims;
using SGP.Shared.Records;

namespace SGP.Shared.Interfaces
{
    public interface ITokenClaimsService
    {
        AccessToken GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
    }
}