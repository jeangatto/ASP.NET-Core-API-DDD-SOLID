using GraphQL;
using GraphQL.Types;
using SGP.Application.Interfaces;
using SGP.Application.Requests;
using SGP.GraphQL.Extensions;
using SGP.GraphQL.Types;

namespace SGP.GraphQL.Queries
{
    public class UserQuery : ObjectGraphType
    {
        public UserQuery(IUserService service)
        {
            FieldAsync<UserType>(
                  name: "userById",
                  arguments: new QueryArguments(
                      new QueryArgument<NonNullGraphType<IdGraphType>>
                      {
                          Name = "id"
                      }),
                  resolve: async context =>
                  {
                      var id = context.GetArgument<string>("id");
                      var request = new GetByIdRequest(id);

                      var result = await service.GetByIdAsync(request);
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
