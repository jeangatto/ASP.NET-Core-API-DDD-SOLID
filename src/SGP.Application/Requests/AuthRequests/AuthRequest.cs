using SGP.Application.Requests.Common;

namespace SGP.Application.Requests.AuthRequests
{
    public class AuthRequest : BaseRequest
    {
        public AuthRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public AuthRequest()
        {
        }

        public string Email { get; set; }
        public string Password { get; set; }
    }
}