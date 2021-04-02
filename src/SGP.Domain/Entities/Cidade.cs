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

        public string Ibge { get; }
        public string Estado { get; }
        public string Nome { get; }
    }
}