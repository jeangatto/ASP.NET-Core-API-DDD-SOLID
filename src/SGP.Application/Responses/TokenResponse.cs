using SGP.Shared.Messages;
using System;

namespace SGP.Application.Responses
{
    public sealed class TokenResponse : BaseResponse
    {
        public TokenResponse(bool authenticated, string accessToken, DateTime created, DateTime expiration, string refreshToken, short seconds)
        {
            Authenticated = authenticated;
            AccessToken = accessToken;
            Created = created;
            Expiration = expiration;
            RefreshToken = refreshToken;
            Seconds = seconds;
        }

        public bool Authenticated { get; }
        public string AccessToken { get; }
        public DateTime Created { get; }
        public DateTime Expiration { get; }
        public string RefreshToken { get; }
        public short Seconds { get; }
    }
}
