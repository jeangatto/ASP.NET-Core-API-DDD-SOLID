namespace SGP.Tests.Models
{
    public class GraphQLError
    {
        public GraphQLError(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }

        public override string ToString()
        {
            return Message;
        }
    }
}
