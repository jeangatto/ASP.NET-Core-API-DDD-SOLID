using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using SGP.Shared.Helpers;
using SGP.Shared.Messages;

namespace SGP.Application.Requests.AuthenticationRequests
{
    public class RefreshTokenRequest : BaseRequest
    {
        public RefreshTokenRequest(string token) => Token = token;

        /// <summary>
        /// Token de atualização (RefreshToken)
        /// </summary>
        [Required]
        public string Token { get; }

        public async override Task ValidateAsync()
            => ValidationResult = await ValidatorHelper.ValidateAsync<RefreshTokenRequestValidator>(this);
    }
}