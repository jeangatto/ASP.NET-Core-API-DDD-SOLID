using SGP.Application.Requests.Common;

namespace SGP.Application.Requests.UserRequests
{
    public class CreateUserRequest : BaseRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
