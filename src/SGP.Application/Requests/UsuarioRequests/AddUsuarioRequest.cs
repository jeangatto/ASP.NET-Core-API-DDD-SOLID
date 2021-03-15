using SGP.Shared.Messages;

namespace SGP.Application.Requests.UsuarioRequests
{
    public class AddUsuarioRequest : BaseRequest
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        public override void Validate() => ValidateAndAddToNotifiable(this);
    }
}
