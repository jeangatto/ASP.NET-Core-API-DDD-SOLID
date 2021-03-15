using SGP.Shared.Messages;

namespace SGP.Application.Requests.CidadeRequests
{
    public class GetByIbgeRequest : BaseRequest<GetByIbgeRequest>
    {
        public GetByIbgeRequest(string ibge)
        {
            Ibge = ibge;
        }

        public string Ibge { get; }
    }
}
