using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using SGP.PublicApi.GraphQL.Queries;
using System;

namespace SGP.PublicApi.GraphQL.Schemas
{
    public class EstadoSchema : Schema
    {
        public EstadoSchema(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<EstadoQuery>();
        }
    }
}
