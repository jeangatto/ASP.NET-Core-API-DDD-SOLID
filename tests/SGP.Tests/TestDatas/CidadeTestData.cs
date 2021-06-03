using Xunit;

namespace SGP.Tests.TestDatas
{
    public class FiltrarPorIbgeTestData : TheoryData<int, string, string, string>
    {
        public FiltrarPorIbgeTestData()
        {
            Add(3557105, "Votuporanga", "SP", "Sudeste");
            Add(3550308, "São Paulo", "SP", "Sudeste");
            Add(3506003, "Bauru", "SP", "Sudeste");
            Add(3304557, "Rio de Janeiro", "RJ", "Sudeste");
            Add(4205407, "Florianópolis", "SC", "Sul");
        }
    }

    public class FiltrarPorUfTestData : TheoryData<string, int>
    {
        public FiltrarPorUfTestData()
        {
            Add("AC", 22);  // Acre
            Add("AL", 102); // Alagoas
            Add("AM", 62);  // Amazonas
            Add("AP", 16);  // Amapá
            Add("BA", 417); // Bahia
            Add("CE", 184); // Ceará
            Add("ES", 78);  // Espírito Santo
            Add("GO", 246); // Goiás
            Add("MA", 217); // Maranhão
            Add("MG", 853); // Minas Gerais
            Add("MS", 79);  // Mato Grosso do Sul
            Add("MT", 141); // Mato Grosso
            Add("PA", 144); // Pará
            Add("PB", 223); // Paraíba
            Add("PE", 185); // Pernambuco
            Add("PI", 224); // Piauí
            Add("PR", 399); // Paraná
            Add("RJ", 92);  // Rio de Janeiro
            Add("RN", 167); // Rio Grande do Norte
            Add("RO", 52);  // Rondônia
            Add("RR", 15);  // Roraima
            Add("RS", 497); // Rio Grande do Sul
            Add("SC", 295); // Santa Catarina
            Add("SE", 75);  // Sergipe
            Add("SP", 645); // São Paulo
            Add("TO", 139); // Tocantins
        }
    }
}
