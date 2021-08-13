namespace SGP.Tests.Models
{
    public class GraphQLError
    {
        public GraphQLError(string message)
        {
            Message = message;
        }

        public string Message { get; }

        public override string ToString()
        {
            return Message;
        }
    }
}
