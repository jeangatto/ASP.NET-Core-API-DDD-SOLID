using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using SGP.Shared;
using SGP.Shared.Messages;

namespace SGP.Application.Requests.AuthenticationRequests;

public class LogInRequest(string email, string password) : BaseRequestWithValidation
{
    [Required]
    [MaxLength(100)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; } = email;

    [Required]
    [MinLength(4)]
    [DataType(DataType.Password)]
    public string Password { get; } = password;

    public override async Task ValidateAsync() =>
        ValidationResult = await LazyValidator.ValidateAsync<LogInRequestValidator, LogInRequest>(this);
}