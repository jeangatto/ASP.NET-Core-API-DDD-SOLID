using SGP.Application.Responses.Common;
using System;

namespace SGP.Application.Responses
{
    public sealed class TokenResponse : BaseResponse
    {
        public TokenResponse(
            bool authenticated,
            string accessToken,
            DateTime created,
            DateTime expiration,
            string refreshToken)
        {
            Authenticated = authenticated;
            AccessToken = accessToken;
            Created = created;
            Expiration = expiration;
            RefreshToken = refreshToken;
        }

        public bool Authenticated { get; private init; }
        public string AccessToken { get; private init; }
        public DateTime Created { get; private init; }
        public DateTime Expiration { get; private init; }
        public string RefreshToken { get; private init; }
        public int SecondsToExpire => (int)Expiration.Subtract(Created).TotalSeconds;
    }
}
