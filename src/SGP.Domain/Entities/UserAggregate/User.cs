using Ardalis.GuardClauses;
using SGP.Domain.ValueObjects;
using SGP.Shared.Entities;
using SGP.Shared.Interfaces;
using System;
using System.Collections.Generic;

namespace SGP.Domain.Entities.UserAggregate
{
    public class User : BaseEntity, IAggregateRoot
    {
        private readonly List<RefreshToken> _refreshTokens = new();

        public User(string name, Email email, string passwordHash)
        {
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
        }

        private User() // ORM
        {
        }

        public string Name { get; private set; }
        public Email Email { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime? LastAccessAt { get; private set; }
        public DateTime? LockExpires { get; private set; }
        public short FailuresNum { get; private set; }

        public IReadOnlyList<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

        /// <summary>
        /// Indica se a conta do usuário está bloqueada.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>Verdadeiro se a conta estiver bloqueada; caso contrário, falso.</returns>
        public bool IsLocked(IDateTime dateTime)
        {
            Guard.Against.Null(dateTime, nameof(dateTime));
            return LockExpires > dateTime.Now;
        }

        /// <summary>
        /// Adiciona um novo token de acesso para o usuário.
        /// </summary>
        /// <param name="refreshToken"></param>
        public void AddRefreshToken(RefreshToken refreshToken)
        {
            Guard.Against.Null(refreshToken, nameof(refreshToken));
            _refreshTokens.Add(refreshToken);
        }

        public void UpdateName(string name)
        {
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Name = name;
        }

        public void UpdateEmail(Email email)
        {
            Guard.Against.Null(email, nameof(email));
            Email = email;
        }

        public void SetPasswordHash(string passwordHash)
        {
            Guard.Against.NullOrWhiteSpace(passwordHash, nameof(passwordHash));
            PasswordHash = passwordHash;
        }

        public void SetLastAcess(IDateTime dateTime)
        {
            Guard.Against.Null(dateTime, nameof(dateTime));
            LastAccessAt = dateTime.Now;
        }

        /// <summary>
        /// Incremenenta o número de acessos efetuado com falha.
        /// Quando é atingido o limite de acessos a conta será bloqueada por um tempo.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="maximumAttempts">Número máximo de tentativas até a conta ser bloqueada.</param>
        /// <param name="secondsBlocked">Segundos em que a conta ficará bloqueada.</param>
        public void IncrementFailuresNum(IDateTime dateTime, short maximumAttempts, short secondsBlocked)
        {
            Guard.Against.Null(dateTime, nameof(dateTime));
            Guard.Against.NegativeOrZero(maximumAttempts, nameof(maximumAttempts));
            Guard.Against.NegativeOrZero(secondsBlocked, nameof(secondsBlocked));

            if (IsLocked(dateTime))
            {
                return;
            }

            FailuresNum++;

            if (FailuresNum == maximumAttempts)
            {
                FailuresNum = 0;
                LockExpires = dateTime.Now.AddSeconds(secondsBlocked);
            }
        }
    }
}