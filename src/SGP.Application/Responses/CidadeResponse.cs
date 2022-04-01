using SGP.Shared.Messages;

namespace SGP.Application.Responses;

public class CidadeResponse : BaseResponse
{
    public string Regiao { get; set; }
    public string Estado { get; set; }
    public string Uf { get; set; }
    public string Nome { get; set; }
    public int Ibge { get; set; }
}