using SGP.Shared.Records;
using System.Collections.Generic;
using System.Security.Claims;

namespace SGP.Shared.Interfaces
{
    public interface ITokenClaimsService
    {
        AccessToken GenerateAccessToken(IEnumerable<Claim> claims);
        RefreshToken GenerateRefreshToken();
    }
}