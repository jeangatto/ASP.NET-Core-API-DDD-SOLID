using SGP.Shared.Entities;
using System;

namespace SGP.Domain.Entities
{
    public class Cidade : BaseEntity
    {
        public Cidade(string nome, int ibge)
        {
            Nome = nome;
            Ibge = ibge;
        }

        private Cidade()
        {
        }

        public Guid EstadoId { get; private set; }
        public string Nome { get; private set; }
        public int Ibge { get; private set; }

        public Estado Estado { get; private set; }
    }
}