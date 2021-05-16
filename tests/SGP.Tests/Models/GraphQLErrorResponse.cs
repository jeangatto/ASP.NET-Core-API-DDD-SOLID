namespace SGP.Tests.Models
{
    public class GraphQLErrorResponse
    {
        public GraphQLErrorResponse(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}
