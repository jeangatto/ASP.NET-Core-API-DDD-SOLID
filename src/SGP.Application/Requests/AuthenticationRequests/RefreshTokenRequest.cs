using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using SGP.Shared;
using SGP.Shared.Messages;

namespace SGP.Application.Requests.AuthenticationRequests;

public class RefreshTokenRequest(string token) : BaseRequestWithValidation
{

    /// <summary>
    /// Token de atualização (RefreshToken)
    /// </summary>
    [Required]
    public string Token { get; } = token;

    public override async Task ValidateAsync() =>
        ValidationResult = await LazyValidator.ValidateAsync<RefreshTokenRequestValidator>(this);
}