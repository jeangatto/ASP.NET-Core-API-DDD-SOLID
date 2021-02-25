using SGP.Domain.Enums;
using SGP.Shared.Entities;
using System.Collections.Generic;

namespace SGP.Domain.Entities
{
    public class Estado : GuidEntityKey
    {
        public Estado(string nome, string uf, short ibge, Regiao regiao)
        {
            Nome = nome;
            Sigla = uf;
            Ibge = ibge;
            Regiao = regiao;
        }

        private Estado()
        {
        }

        public string Nome { get; private set; }
        public string Sigla { get; private set; }
        public int Ibge { get; private set; }
        public Regiao Regiao { get; private set; }

        public IReadOnlyList<Cidade> Cidades { get; private set; }
    }
}