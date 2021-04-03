namespace SGP.Application.Requests.CidadeRequests
{
    public class GetAllByEstadoRequest
    {
        public GetAllByEstadoRequest(string estadoSigla)
        {
            EstadoSigla = estadoSigla;
        }

        public string EstadoSigla { get; }
    }
}