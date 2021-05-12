using GraphQL.Types;
using SGP.Application.Responses;

namespace SGP.GraphQL.Types
{
    public class UserType : ObjectGraphType<UserResponse>
    {
        public UserType()
        {
            Field(x => x.Id, type: typeof(IdGraphType))
                .Description("Identificador global único.");

            Field(x => x.Name)
                .Description("Nome do usuário.");

            Field(x => x.Email)
                .Description("Login de acesso.");
        }
    }
}
