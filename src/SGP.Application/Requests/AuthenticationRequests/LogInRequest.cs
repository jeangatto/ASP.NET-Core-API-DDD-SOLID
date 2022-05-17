using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using SGP.Shared.Helpers;
using SGP.Shared.Messages;

namespace SGP.Application.Requests.AuthenticationRequests;

public class LogInRequest : BaseRequest
{
    public LogInRequest(string email, string password)
    {
        Email = email;
        Password = password;
    }

    [Required]
    [MaxLength(100)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; }

    [Required]
    [MinLength(4)]
    [DataType(DataType.Password)]
    public string Password { get; }

    public override async Task ValidateAsync()
    {
        ValidationResult = await ValidatorHelper.ValidateAsync<LogInRequestValidator>(this);
    }
}