using SGP.Shared.Entities;
using SGP.Shared.Interfaces;
using System;

namespace SGP.Domain.Entities.UsuarioAggregate
{
    public class RefreshToken : BaseEntity
    {
        /// <summary>
        /// Comprimento do refreh token.
        /// </summary>
        public const int MAX_TOKEN_SIZE = 2048;

        public RefreshToken(string token, DateTime createdAt, DateTime expireAt)
        {
            Token = token;
            CreatedAt = createdAt;
            ExpireAt = expireAt;
        }

        private RefreshToken() // ORM
        {
        }

        public Guid UsuarioId { get; private set; }
        public string Token { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime ExpireAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public string ReplacedByToken { get; private set; }

        public Usuario Usuario { get; private set; }

        /// <summary>
        /// Indica se o token está expirado.
        /// </summary>
        /// <param name="dataTime"></param>
        /// <returns>Verdadeiro se o token estiver expirado; caso contrário, falso.</returns>
        public bool IsExpired(IDateTime dataTime)
        {
            return dataTime.Now >= ExpireAt || RevokedAt.HasValue;
        }

        public void Revoke(string newToken, DateTime revokedAt)
        {
            ReplacedByToken = newToken;
            RevokedAt = revokedAt;
        }
    }
}
