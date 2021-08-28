using System;
using Ardalis.GuardClauses;
using SGP.Shared.Entities;
using SGP.Shared.Interfaces;

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

        /// <summary>
        /// Identificação do usuário.
        /// </summary>
        public Guid UsuarioId { get; private set; }

        /// <summary>
        /// Token de atualização.
        /// </summary>
        public string Token { get; private set; }

        /// <summary>
        /// Data da criação do Token.
        /// </summary>
        public DateTime CriadoEm { get; private set; }

        /// <summary>
        /// Data do vencimento do token.
        /// </summary>
        public DateTime ExpiraEm { get; private set; }

        /// <summary>
        /// Data da revogação (cancelamento) do token.
        /// </summary>
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
        /// <param name="dataRevogacao">Data da revogação.</param>
        public void RevogarToken(DateTime dataRevogacao)
        {
            if (!EstaRevogado)
                RevogadoEm = dataRevogacao;
        }
    }
}