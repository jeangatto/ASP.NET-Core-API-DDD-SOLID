using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using SGP.GraphQL.Queries;
using System;

namespace SGP.GraphQL.Schemas
{
    public class CitySchema : Schema
    {
        public CitySchema(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<CityQuery>();
        }
    }
}
