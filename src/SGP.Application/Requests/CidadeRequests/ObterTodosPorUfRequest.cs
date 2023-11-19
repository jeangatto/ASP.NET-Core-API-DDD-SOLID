using System.Threading.Tasks;
using SGP.Shared;
using SGP.Shared.Messages;

namespace SGP.Application.Requests.CidadeRequests;

public class ObterTodosPorUfRequest(string uf) : BaseRequestWithValidation
{
    public string Uf { get; } = uf;

    public override async Task ValidateAsync() =>
        ValidationResult = await LazyValidator.ValidateAsync<ObterTodosPorUfRequestValidator>(this);
}