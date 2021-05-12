using GraphQL;
using GraphQL.Types;
using SGP.Application.Interfaces;
using SGP.Application.Requests.UserRequests;
using SGP.GraphQL.Extensions;
using SGP.GraphQL.Types;
using SGP.GraphQL.Types.Inputs;

namespace SGP.GraphQL.Mutations
{
    public class UserMutation : ObjectGraphType
    {
        public UserMutation(IUserService service)
        {
            FieldAsync<UserType>(
                name: "createUser",
                arguments: new QueryArguments(
                    new QueryArgument<CreateUserInputType>()
                    {
                        Name = "user"
                    }),
                resolve: async context =>
                {
                    var request = context.GetArgument<CreateUserRequest>("user");

                    var result = await service.CreateAsync(request);
                    if (result.IsFailed)
                    {
                        result.ToExecutionError(context);
                        return null;
                    }

                    return result.Value;
                });
        }
    }
}
