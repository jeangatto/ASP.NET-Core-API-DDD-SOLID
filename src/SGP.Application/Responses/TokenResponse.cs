using System;
using SGP.Shared.Messages;

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

        public string AccessToken { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime Expiration { get; private set; }
        public string RefreshToken { get; private set; }
        public int ExpiresIn => (int)Created.Subtract(Expiration).TotalSeconds;
    }
}