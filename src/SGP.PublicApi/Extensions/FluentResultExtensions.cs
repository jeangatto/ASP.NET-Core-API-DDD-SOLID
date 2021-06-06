using FluentResults;
using GraphQL;

namespace SGP.PublicApi.Extensions
{
    public static class FluentResultExtensions
    {
        public static Result<T> ToExecutionError<T>(this Result<T> result,
            IResolveFieldContext<object> context)
        {
            foreach (var error in result.Errors)
            {
                context.Errors.Add(new ExecutionError(error.Message));
            }

            return result;
        }
    }
}
