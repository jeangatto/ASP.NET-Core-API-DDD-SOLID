using SGP.Shared.Messages;

namespace SGP.Application.Requests.AuthRequests
{
    public class RefreshTokenRequest : BaseRequest
    {
        public RefreshTokenRequest(string token) => Token = token;

        public string Token { get; }

        public override void Validate()
        {
            ValidationResult = new RefreshTokenRequestValidator().Validate(this);
        }

        public override string ToString() => Token;
    }
}
