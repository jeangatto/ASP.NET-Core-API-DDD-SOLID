using SGP.Shared.Extensions;
using SGP.Shared.Messages;

namespace SGP.Application.Requests.AuthRequests
{
    public class RefreshTokenRequest : BaseRequest
    {
        public string Token { get; set; }

        public override void Validate()
        {
            new RefreshTokenRequestValidator()
                .Validate(this)
                .AddToNotifiable(this);
        }
    }
}
