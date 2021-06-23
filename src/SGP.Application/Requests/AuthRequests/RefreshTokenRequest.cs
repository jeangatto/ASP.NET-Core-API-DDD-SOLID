using SGP.Shared.Messages;
using System.ComponentModel.DataAnnotations;

namespace SGP.Application.Requests.AuthRequests
{
    public class RefreshTokenRequest : BaseRequest
    {
        public RefreshTokenRequest(string token)
        {
            Token = token;
        }

        [Required]
        public string Token { get; private init; }

        public override void Validate()
        {
            ValidationResult = new RefreshTokenRequestValidator().Validate(this);
        }
    }
}
