using SGP.Application.Requests.Common;

namespace SGP.Application.Requests.CityRequests
{
    public class GetByIbgeRequest : BaseRequest
    {
        public GetByIbgeRequest(string ibge) => Ibge = ibge;

        public string Ibge { get; }

        public override string ToString() => Ibge;
    }
}
