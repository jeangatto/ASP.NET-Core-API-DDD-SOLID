using SGP.Application.Requests.Common;

namespace SGP.Application.Requests.UserRequests
{
    public class CreateUserRequest : BaseRequest
    {
        public CreateUserRequest(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }

        public CreateUserRequest()
        {
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
