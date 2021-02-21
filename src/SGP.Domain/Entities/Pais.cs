using SGP.Shared.Entities;
using System.Collections.Generic;

namespace SGP.Domain.Entities
{
    public class Pais : BaseEntity
    {
        public Pais(string nome, string sigla, short bacen)
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
        public short Bacen { get; private set; }

        public IReadOnlyCollection<Estado> Estados { get; private set; }
    }
}
