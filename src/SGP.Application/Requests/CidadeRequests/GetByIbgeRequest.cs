using SGP.Shared.Extensions;
using SGP.Shared.Messages;

namespace SGP.Application.Requests.CidadeRequests
{
    public class GetByIbgeRequest : BaseRequest
    {
        public GetByIbgeRequest(string ibge)
        {
            Ibge = ibge;
        }

        public string Ibge { get; }

        public override void Validate()
        {
            new GetByIbgeRequestValidator().Validate(this).AddToNotifiable(this);
        }
    }
}
