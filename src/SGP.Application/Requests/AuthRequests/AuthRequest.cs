using SGP.Shared.Messages;
using System.ComponentModel.DataAnnotations;

namespace SGP.Application.Requests.AuthRequests
{
    public class AuthRequest : BaseRequest
    {
        public AuthRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; private init; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; private init; }

        public override void Validate()
        {
            ValidationResult = new AuthRequestValidator().Validate(this);
        }
    }
}