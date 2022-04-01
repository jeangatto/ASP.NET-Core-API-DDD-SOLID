using SGP.Shared.Messages;

namespace SGP.Application.Responses;

public class EstadoResponse : BaseResponse
{
    public string Regiao { get; set; }
    public string Uf { get; set; }
    public string Nome { get; set; }
}