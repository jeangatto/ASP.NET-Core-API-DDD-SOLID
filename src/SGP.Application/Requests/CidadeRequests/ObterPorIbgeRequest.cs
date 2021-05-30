using SGP.Application.Requests.Common;

namespace SGP.Application.Requests.CidadeRequests
{
    public class ObterPorIbgeRequest : BaseRequest
    {
        public int Ibge { get; }

        public ObterPorIbgeRequest(int ibge) => Ibge = ibge;

        public override string ToString() => Ibge.ToString();
    }
}