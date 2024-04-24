using System.Threading.Tasks;
using SGP.Shared;
using SGP.Shared.Messages;

namespace SGP.Application.Requests.CidadeRequests;

public class ObterPorIbgeRequest(int ibge) : BaseRequestWithValidation
{
    public int Ibge { get; } = ibge;

    public override async Task ValidateAsync() =>
        ValidationResult = await LazyValidator.ValidateAsync<ObterPorIbgeRequestValidator, ObterPorIbgeRequest>(this);
}