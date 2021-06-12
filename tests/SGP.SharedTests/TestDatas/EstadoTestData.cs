using Xunit;

namespace SGP.SharedTests.TestDatas
{
    public abstract class EstadoTestData
    {
        /// <summary>
        /// T1 = Nome da região,
        /// T2 = Total de estados da região
        /// </summary>
        public class FiltrarEstadoPorRegiaoData : TheoryData<string, int>
        {
            public FiltrarEstadoPorRegiaoData()
            {
                Add("Nordeste", 9);
                Add("Sudeste", 4);
                Add("Sul", 3);
                Add("Centro-Oeste", 4);
                Add("Norte", 7);
            }
        }
    }
}
