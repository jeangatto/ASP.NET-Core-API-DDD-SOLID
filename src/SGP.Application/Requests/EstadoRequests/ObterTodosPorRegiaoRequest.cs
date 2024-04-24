using System.Threading.Tasks;
using SGP.Shared;
using SGP.Shared.Messages;

namespace SGP.Application.Requests.EstadoRequests;

public class ObterTodosPorRegiaoRequest(string regiao) : BaseRequestWithValidation
{
    public string Regiao { get; } = regiao;

    public override async Task ValidateAsync() =>
        ValidationResult = await LazyValidator.ValidateAsync<ObterTodosPorRegiaoRequestValidator, ObterTodosPorRegiaoRequest>(this);
}