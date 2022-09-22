using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using SGP.Shared.Helpers;
using SGP.Shared.Messages;

namespace SGP.Application.Requests.AuthenticationRequests;

public class RefreshTokenRequest : BaseRequestWithValidation
{
    public RefreshTokenRequest(string token) => Token = token;

    /// <summary>
    /// Token de atualização (RefreshToken)
    /// </summary>
    [Required]
    public string Token { get; }

    public override async Task ValidateAsync()
        => ValidationResult = await ValidatorHelper.ValidateAsync<RefreshTokenRequestValidator>(this);
}