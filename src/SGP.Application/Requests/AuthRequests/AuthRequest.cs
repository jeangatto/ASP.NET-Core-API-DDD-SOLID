using SGP.Shared.Messages;

namespace SGP.Application.Requests.AuthRequests
{
    public class AuthRequest : BaseRequest
    {
        public AuthRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; }
        public string Password { get; }

        public override void Validate()
        {
            ValidationResult = new AuthRequestValidator().Validate(this);
        }
    }
}