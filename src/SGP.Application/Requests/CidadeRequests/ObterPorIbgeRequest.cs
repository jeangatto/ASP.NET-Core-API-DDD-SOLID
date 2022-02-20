using System.Threading.Tasks;
using SGP.Shared.Helpers;
using SGP.Shared.Messages;

namespace SGP.Application.Requests.CidadeRequests
{
    public class ObterPorIbgeRequest : BaseRequest
    {
        public ObterPorIbgeRequest(int ibge) => Ibge = ibge;

        public int Ibge { get; }

        public async override Task ValidateAsync()
            => ValidationResult = await ValidatorHelper.ValidateAsync<ObterPorIbgeRequestValidator>(this);
    }
}