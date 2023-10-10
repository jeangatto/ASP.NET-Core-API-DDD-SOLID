using System.Threading.Tasks;
using SGP.Shared;
using SGP.Shared.Messages;

namespace SGP.Application.Requests.EstadoRequests;

public class ObterTodosPorRegiaoRequest : BaseRequestWithValidation
{
    public ObterTodosPorRegiaoRequest(string regiao) =>
        Regiao = regiao;

    public string Regiao { get; }

    public override async Task ValidateAsync() =>
        ValidationResult = await LazyValidator.ValidateAsync<ObterTodosPorRegiaoRequestValidator>(this);
}