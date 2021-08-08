using System;
using SGP.Shared.Messages;

namespace SGP.Application.Responses
{
    public sealed class TokenResponse : BaseResponse
    {
        public TokenResponse(string accessToken, DateTime created, DateTime expiration, string refreshToken)
        {
            AccessToken = accessToken;
            Created = created;
            Expiration = expiration;
            RefreshToken = refreshToken;
        }

        public string AccessToken { get; }
        public DateTime Created { get; }
        public DateTime Expiration { get; }
        public string RefreshToken { get; }
        public int ExpiresIn => (int)Expiration.Subtract(Created).TotalSeconds;
    }
}