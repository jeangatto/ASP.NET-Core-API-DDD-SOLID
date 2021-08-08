using SGP.Shared.Helpers;
using SGP.Shared.Messages;

namespace SGP.Application.Requests.CidadeRequests
{
    public class ObterPorIbgeRequest : BaseRequest
    {
        public ObterPorIbgeRequest(int ibge)
        {
            Ibge = ibge;
        }

        public int Ibge { get; }

        public override void Validate()
        {
            ValidationResult = ValidatorHelper.Validate<ObterPorIbgeRequestValidator>(this);
        }
    }
}