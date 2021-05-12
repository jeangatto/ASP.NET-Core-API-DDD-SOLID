using FluentResults;
using GraphQL;

namespace SGP.GraphQL.Extensions
{
    public static class FluentResultExtensions
    {
        public static void ToExecutionError<T>(this Result<T> result, IResolveFieldContext<object> context)
        {
            foreach (var error in result.Errors)
            {
                context.Errors.Add(new ExecutionError(error.Message));
            }
        }
    }
}
