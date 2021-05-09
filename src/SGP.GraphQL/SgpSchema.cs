using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using SGP.Infrastructure.GraphQL.Querys;
using System;

namespace SGP.Infrastructure.GraphQL
{
    public class SgpSchema : Schema
    {
        public SgpSchema(IServiceProvider services) : base(services)
        {
            Query = services.GetRequiredService<CityQuery>();
        }
    }
}
