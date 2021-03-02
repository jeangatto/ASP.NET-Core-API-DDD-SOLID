using SGP.Shared.Entities;
using System.Collections.Generic;

namespace SGP.Domain.Entities
{
    public class Pais : BaseEntity
    {
        public Pais(string nome, string sigla, int bacen)
        {
            Nome = nome;
            Sigla = sigla;
            Bacen = bacen;
        }

        private Pais()
        {
        }

        public string Nome { get; private set; }
        public string Sigla { get; private set; }
        public int Bacen { get; private set; }

        public IReadOnlyList<Estado> Estados { get; private set; }
    }
}
