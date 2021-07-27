namespace SGP.PublicApi.GraphQL.Queries
{
    using Application.Interfaces;
    using Constants;
    using global::GraphQL.Types;
    using Types;

    public class EstadoQuery : ObjectGraphType
    {
        public EstadoQuery(IEstadoService service)
        {
            FieldAsync<ListGraphType<EstadoType>>(
               QueryNames.ListarEstados,
               resolve: async _ => (await service.ObterTodosAsync()).Value);
        }
    }
}
