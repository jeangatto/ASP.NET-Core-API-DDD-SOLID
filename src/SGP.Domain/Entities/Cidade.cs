namespace SGP.Domain.Entities
{
    public class Cidade
    {
        public Cidade(string ibge, string estado, string nome)
        {
            Ibge = ibge;
            Estado = estado;
            Nome = nome;
        }

        private Cidade() // ORM
        {
        }

        public string Ibge { get; private init; }
        public string Estado { get; private init; }
        public string Nome { get; private init; }
    }
}