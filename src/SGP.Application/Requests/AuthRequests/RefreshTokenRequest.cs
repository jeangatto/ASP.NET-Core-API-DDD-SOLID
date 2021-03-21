using SGP.Shared.Messages;

namespace SGP.Application.Requests.AuthRequests
{
    public class RefreshTokenRequest : BaseRequestWithValidator<RefreshTokenRequest>
    {
        public string Token { get; set; }
    }
}
