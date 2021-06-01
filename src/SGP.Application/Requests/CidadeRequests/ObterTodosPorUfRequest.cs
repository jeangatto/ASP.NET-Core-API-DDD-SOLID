using SGP.Application.Requests.Common;

namespace SGP.Application.Requests.CidadeRequests
{
    public class ObterTodosPorUfRequest : BaseRequest
    {
        public ObterTodosPorUfRequest(string uf)
        {
            Uf = uf?.ToUpperInvariant();
        }

        public string Uf { get; }

        public override void Validate()
        {
            ValidationResult = new ObterTodosPorUfRequestValidator().Validate(this);
        }
    }
}