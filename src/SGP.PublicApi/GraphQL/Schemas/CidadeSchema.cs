using System;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using SGP.PublicApi.GraphQL.Queries;

namespace SGP.PublicApi.GraphQL.Schemas
{
    public class CidadeSchema : Schema
    {
        public CidadeSchema(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<CidadeQuery>();
        }
    }
}