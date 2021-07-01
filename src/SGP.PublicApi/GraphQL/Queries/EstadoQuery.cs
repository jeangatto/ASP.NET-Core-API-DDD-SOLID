using GraphQL.Types;
using SGP.Application.Interfaces;
using SGP.PublicApi.GraphQL.Constants;
using SGP.PublicApi.GraphQL.Types;

namespace SGP.PublicApi.GraphQL.Queries
{
    public class EstadoQuery : ObjectGraphType
    {
        public EstadoQuery(IEstadoService service)
        {
            FieldAsync<ListGraphType<EstadoType>>(
               name: QueryNames.ListarEstados,
               resolve: async _ => (await service.ObterTodosAsync()).Value);
        }
    }
}
