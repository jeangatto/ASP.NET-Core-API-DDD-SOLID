using SGP.Shared.Entities;
using System;
using System.Collections.Generic;

namespace SGP.Domain.Entities
{
    public class Estado : BaseEntity
    {
        public Estado(Guid paisId, string nome, string uf, int ibge, string ddd)
        {
            PaisId = paisId;
            Nome = nome;
            UF = uf;
            Ibge = ibge;
            DDD = ddd;
        }

        private Estado()
        {
        }

        public Guid PaisId { get; private set; }
        public string Nome { get; private set; }
        public string UF { get; private set; }
        public int Ibge { get; private set; }
        public string DDD { get; private set; }

        public Pais Pais { get; private set; }
        public IReadOnlyCollection<Cidade> Cidades { get; private set; }
    }
}
