using GraphQL.Types;
using SGP.Application.Requests.UserRequests;

namespace SGP.GraphQL.Types.Inputs
{
    public class CreateUserInputType : InputObjectGraphType<CreateUserRequest>
    {
        public CreateUserInputType()
        {
            Name = "CreateUserInput";

            Field(x => x.Name);
            Field(x => x.Email);
            Field(x => x.Password);
        }
    }
}
