using System;

namespace SGP.Shared.Entities
{
    public class JWToken
    {
        public JWToken(
            string accessToken,
            string refreshToken,
            DateTime createdAt,
            DateTime expiresAt,
            short secondsToExpire)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            CreatedAt = createdAt;
            ExpiresAt = expiresAt;
            SecondsToExpire = secondsToExpire;
        }

        public string AccessToken { get; }
        public string RefreshToken { get; }
        public DateTime CreatedAt { get; }
        public DateTime ExpiresAt { get; }
        public short SecondsToExpire { get; }
    }
}
