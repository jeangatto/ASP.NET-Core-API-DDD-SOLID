using Ardalis.GuardClauses;
using SGP.Shared.Entities;
using SGP.Shared.Interfaces;
using System;

namespace SGP.Domain.Entities
{
    public class TokenAcesso : BaseEntity
    {
        public TokenAcesso(string token, DateTime criadoEm, DateTime expiraEm)
        {
            Token = token;
            CriadoEm = criadoEm;
            ExpiraEm = expiraEm;
        }

        private TokenAcesso() // ORM
        {
        }

        public Guid UsuarioId { get; private set; }
        public string Token { get; private set; }
        public DateTime CriadoEm { get; private set; }
        public DateTime ExpiraEm { get; private set; }
        public DateTime? RevogadoEm { get; private set; }

        public Usuario Usuario { get; private set; }

        /// <summary>
        /// Indica se o Token foi revogado (cancelado).
        /// </summary>
        public bool EstaRevogado => RevogadoEm.HasValue;

        /// <summary>
        /// Indica se o token está expirado ou revogado.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>Verdadeiro se o token estiver expirado ou revogado; caso contrário, falso.</returns>
        public bool EstaValido(IDateTime dateTime)
        {
            Guard.Against.Null(dateTime, nameof(dateTime));
            return dateTime.Now >= ExpiraEm || EstaRevogado;
        }

        /// <summary>
        /// Revoga (cancela) o token.
        /// </summary>
        /// <param name="dataRevogacao"></param>
        public void RevogarToken(DateTime dataRevogacao)
        {
            if (!EstaRevogado)
            {
                RevogadoEm = dataRevogacao;
            }
        }
    }
}
