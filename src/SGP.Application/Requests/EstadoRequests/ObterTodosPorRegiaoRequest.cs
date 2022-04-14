using System.Threading.Tasks;
using SGP.Shared.Helpers;
using SGP.Shared.Messages;

namespace SGP.Application.Requests.EstadoRequests;

public class ObterTodosPorRegiaoRequest : BaseRequest
{
    public ObterTodosPorRegiaoRequest(string regiao) => Regiao = regiao;

    public string Regiao { get; }

    public override async Task ValidateAsync()
    {
        ValidationResult = await ValidatorHelper.ValidateAsync<ObterTodosPorRegiaoRequestValidator>(this);
    }
}