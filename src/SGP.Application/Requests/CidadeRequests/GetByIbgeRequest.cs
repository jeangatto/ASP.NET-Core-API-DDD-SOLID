namespace SGP.Application.Requests.CidadeRequests
{
    public class GetByIbgeRequest
    {
        public GetByIbgeRequest(string ibge)
        {
            Ibge = ibge;
        }

        public string Ibge { get; }
    }
}
