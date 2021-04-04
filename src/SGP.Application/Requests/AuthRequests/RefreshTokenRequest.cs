using SGP.Application.Requests.Common;

namespace SGP.Application.Requests.AuthRequests
{
    public class RefreshTokenRequest : BaseRequest
    {
        public string Token { get; set; }
    }
}
