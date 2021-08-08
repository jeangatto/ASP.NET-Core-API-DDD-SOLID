using System.ComponentModel.DataAnnotations;
using SGP.Shared.Helpers;
using SGP.Shared.Messages;

namespace SGP.Application.Requests.AuthenticationRequests
{
    public class LogInRequest : BaseRequest
    {
        public LogInRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(4)]
        public string Password { get; }

        public override void Validate()
        {
            ValidationResult = ValidatorHelper.Validate<LogInRequestValidator>(this);
        }
    }
}