using SGP.Shared.Entities;
using System.Collections.Generic;

namespace SGP.Domain.Entities
{
    public class Regiao : BaseEntity
    {
        public Regiao(string nome) => Nome = nome;

        public Regiao() // ORM
        {
        }

        public string Nome { get; private set; }

        public ICollection<Estado> Estados { get; private set; }
    }
}
