using SGP.Shared.Extensions;
using SGP.Shared.Messages;
using SGP.Shared.Utils;

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
            FluentValidationUtils.GetValidatorInstance<GetByIbgeRequest>()?
                .Validate(this)
                .AddToNotifiable(this);
        }
    }
}
