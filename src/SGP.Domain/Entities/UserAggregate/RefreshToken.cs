using Ardalis.GuardClauses;
using SGP.Shared.Entities;
using SGP.Shared.Interfaces;
using System;

namespace SGP.Domain.Entities.UserAggregate
{
    public class RefreshToken : BaseEntity
    {
        public RefreshToken(string token, DateTime createdAt, DateTime expireAt)
        {
            Token = token;
            CreatedAt = createdAt;
            ExpireAt = expireAt;
        }

        private RefreshToken() // ORM
        {
        }

        public Guid UserId { get; private set; }
        public string Token { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime ExpireAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public string ReplacedByToken { get; private set; }

        public User User { get; private set; }

        /// <summary>
        /// Indica se o token está expirado.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>Verdadeiro se o token estiver expirado; caso contrário, falso.</returns>
        public bool IsExpired(IDateTime dateTime)
        {
            Guard.Against.Null(dateTime, nameof(dateTime));
            return dateTime.Now >= ExpireAt || RevokedAt.HasValue;
        }

        public void Revoke(string newToken, DateTime revokedAt)
        {
            Guard.Against.NullOrWhiteSpace(newToken, nameof(newToken));
            ReplacedByToken = newToken;
            RevokedAt = revokedAt;
        }
    }
}
