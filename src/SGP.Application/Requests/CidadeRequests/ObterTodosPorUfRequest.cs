using SGP.Application.Requests.Common;

namespace SGP.Application.Requests.CidadeRequests
{
    public class ObterTodosPorUfRequest : BaseRequest
    {
        public string Uf { get; }

        public ObterTodosPorUfRequest(string uf) => Uf = uf;

        public override string ToString() => Uf;
    }
}
