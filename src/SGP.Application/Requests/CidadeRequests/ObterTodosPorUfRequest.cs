using System.Threading.Tasks;
using SGP.Shared.Helpers;
using SGP.Shared.Messages;

namespace SGP.Application.Requests.CidadeRequests
{
    public class ObterTodosPorUfRequest : BaseRequest
    {
        public ObterTodosPorUfRequest(string uf) => Uf = uf?.ToUpperInvariant();

        public string Uf { get; }

        public override async Task ValidateAsync()
        {
            ValidationResult = await ValidatorHelper.ValidateAsync<ObterTodosPorUfRequestValidator>(this);
        }
    }
}