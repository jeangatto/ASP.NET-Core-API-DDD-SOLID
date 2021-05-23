using SGP.Shared.Entities;
using System;
using System.Collections.Generic;

namespace SGP.Domain.Entities
{
    public class Estado : BaseEntity
    {
        public Estado(Guid regiaoId, string nome, string uf)
        {
            RegiaoId = regiaoId;
            Nome = nome;
            Uf = uf;
        }

        public Estado() // ORM
        {
        }

        public Guid RegiaoId { get; private set; }
        public string Nome { get; private set; }

        /// <summary>
        /// Unidade Federativa.
        /// </summary>
        public string Uf { get; private set; }

        public Regiao Regiao { get; private set; }
        public ICollection<Cidade> Cidades { get; private set; }
    }
}
