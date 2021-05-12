using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using SGP.GraphQL.Mutations;
using SGP.GraphQL.Queries;
using System;

namespace SGP.GraphQL.Schemas
{
    public class UserSchema : Schema
    {
        public UserSchema(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<UserQuery>();
            Mutation = serviceProvider.GetRequiredService<UserMutation>();
        }
    }
}
