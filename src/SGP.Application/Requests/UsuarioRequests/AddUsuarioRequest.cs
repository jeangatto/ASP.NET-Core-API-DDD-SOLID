using SGP.Shared.Extensions;
using SGP.Shared.Messages;
using SGP.Shared.Utils;

namespace SGP.Application.Requests.UsuarioRequests
{
    public class AddUsuarioRequest : BaseRequest
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        public override void Validate()
        {
            FluentValidationUtils.GetValidatorInstance<AddUsuarioRequest>()?
                .Validate(this)
                .AddToNotifiable(this);
        }
    }
}
