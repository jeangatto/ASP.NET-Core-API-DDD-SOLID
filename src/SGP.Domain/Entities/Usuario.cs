using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using SGP.Domain.ValueObjects;
using SGP.Shared.Entities;
using SGP.Shared.Interfaces;

namespace SGP.Domain.Entities
{
    public class Usuario : BaseEntity, IAggregateRoot
    {
        private readonly List<TokenAcesso> _tokens = new();

        public Usuario(string nome, Email email, string hashSenha)
        {
            Nome = nome;
            Email = email;
            HashSenha = hashSenha;
        }

        private Usuario() // ORM
        {
        }

        public string Nome { get; private set; }
        public Email Email { get; private set; }
        public string HashSenha { get; private set; }
        public DateTime? UltimoAcessoEm { get; private set; }
        public DateTime? BloqueioExpiraEm { get; private set; }
        public int NumeroFalhasAoAcessar { get; private set; }

        public IReadOnlyList<TokenAcesso> Tokens => _tokens.AsReadOnly();

        public void AdicionarToken(TokenAcesso tokenAcesso)
        {
            Guard.Against.Null(tokenAcesso, nameof(tokenAcesso));
            _tokens.Add(tokenAcesso);
        }

        /// <summary>
        /// Indica se a conta do usuário está bloqueada.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>Verdadeiro se a conta estiver bloqueada; caso contrário, falso.</returns>
        public bool EstaBloqueado(IDateTime dateTime)
        {
            Guard.Against.Null(dateTime, nameof(dateTime));
            return BloqueioExpiraEm > dateTime.Now;
        }

        /// <summary>
        /// Incremenenta o número de acessos que falharam.
        /// Quando é atingido o limite de acessos a conta será bloqueada por um tempo.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="numeroTentativas">Número máximo de tentativas até a conta ser bloqueada.</param>
        /// <param name="lockedTimeSpan">Determinado tempo em que a conta ficará bloqueada.</param>
        public void IncrementarFalhas(IDateTime dateTime, short numeroTentativas, TimeSpan lockedTimeSpan)
        {
            Guard.Against.NegativeOrZero(numeroTentativas, nameof(numeroTentativas));
            Guard.Against.Null(lockedTimeSpan, nameof(lockedTimeSpan));

            if (EstaBloqueado(dateTime))
                return;

            NumeroFalhasAoAcessar++;

            if (NumeroFalhasAoAcessar == numeroTentativas)
            {
                NumeroFalhasAoAcessar = 0;
                BloqueioExpiraEm = dateTime.Now.Add(lockedTimeSpan);
            }
        }
    }
}