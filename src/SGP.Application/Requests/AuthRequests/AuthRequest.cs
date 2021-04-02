using SGP.Shared.Extensions;
using SGP.Shared.Messages;

namespace SGP.Application.Requests.AuthRequests
{
    public class AuthRequest : BaseRequest
    {
        public AuthRequest(string email, string senha)
        {
            Email = email;
            Senha = senha;
        }

        public AuthRequest()
        {
        }

        public string Email { get; set; }
        public string Senha { get; set; }

        public override void Validate()
        {
            new AuthRequestValidator()
                .Validate(this)
                .AddToNotifiable(this);
        }
    }
}