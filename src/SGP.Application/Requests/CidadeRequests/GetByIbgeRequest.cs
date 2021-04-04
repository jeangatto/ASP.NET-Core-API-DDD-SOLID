using SGP.Application.Requests.Common;

namespace SGP.Application.Requests.CidadeRequests
{
    public class GetByIbgeRequest : BaseRequest
    {
        public GetByIbgeRequest(string ibge)
        {
            Ibge = ibge;
        }

        public string Ibge { get; }
    }
}
