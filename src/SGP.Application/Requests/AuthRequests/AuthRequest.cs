using System.ComponentModel.DataAnnotations;
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

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; private set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; private set; }

        public override void Validate()
        {
            ValidationResult = new AuthRequestValidator().Validate(this);
        }
    }
}