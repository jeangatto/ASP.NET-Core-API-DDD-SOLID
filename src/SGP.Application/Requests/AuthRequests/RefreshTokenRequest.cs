using SGP.Shared.Messages;

namespace SGP.Application.Requests.AuthRequests
{
    public class RefreshTokenRequest : BaseRequest
    {
        public RefreshTokenRequest(string token)
        {
            Token = token;
        }

        public string Token { get; }

        public override string ToString()
        {
            return Token;
        }

        public override void Validate()
        {
            ValidationResult = new RefreshTokenRequestValidator().Validate(this);
        }
    }
}
