using MediatR;

namespace SGP.Application.Configuration.Queries
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }
}