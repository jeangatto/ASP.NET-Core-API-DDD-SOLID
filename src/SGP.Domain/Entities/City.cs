namespace SGP.Domain.Entities
{
    public class City
    {
        public City(string ibge, string stateAbbr, string name)
        {
            Ibge = ibge;
            StateAbbr = stateAbbr;
            Name = name;
        }

        private City() // ORM
        {
        }

        public string Ibge { get; private init; }
        public string StateAbbr { get; private init; }
        public string Name { get; private init; }
    }
}