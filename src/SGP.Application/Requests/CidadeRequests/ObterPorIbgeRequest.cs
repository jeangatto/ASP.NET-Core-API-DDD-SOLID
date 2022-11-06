using System.Threading.Tasks;
using SGP.Shared;
using SGP.Shared.Messages;

namespace SGP.Application.Requests.CidadeRequests;

public class ObterPorIbgeRequest : BaseRequestWithValidation
{
    public ObterPorIbgeRequest(int ibge) => Ibge = ibge;

    public int Ibge { get; }

    public override async Task ValidateAsync()
        => ValidationResult = await LazyValidator.ValidateAsync<ObterPorIbgeRequestValidator>(this);
}