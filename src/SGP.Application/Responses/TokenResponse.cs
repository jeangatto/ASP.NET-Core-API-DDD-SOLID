using SGP.Shared.Messages;
using System;

namespace SGP.Application.Responses
{
    public sealed class TokenResponse : BaseResponse
    {
        public TokenResponse(
            string accessToken,
            DateTime created,
            DateTime expiration,
            string refreshToken)
        {
            AccessToken = accessToken;
            Created = created;
            Expiration = expiration;
            RefreshToken = refreshToken;
        }

        public string AccessToken { get; private init; }
        public DateTime Created { get; private init; }
        public DateTime Expiration { get; private init; }
        public string RefreshToken { get; private init; }
        public int ExpiresIn => (int)Created.Subtract(Expiration).TotalSeconds;
    }
}
