using System.ComponentModel.DataAnnotations;
using SGP.Shared.Messages;

namespace SGP.Application.Requests.AuthRequests
{
    public class RefreshTokenRequest : BaseRequest
    {
        public RefreshTokenRequest(string token)
        {
            Token = token;
        }

        [Required]
        public string Token { get; private set; }

        public override void Validate()
        {
            ValidationResult = new RefreshTokenRequestValidator().Validate(this);
        }
    }
}