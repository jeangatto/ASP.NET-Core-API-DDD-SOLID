namespace SGP.Tests.Models
{
    public class GraphError
    {
        public GraphError(string message) => Message = message;

        public string Message { get; }

        public override string ToString() => Message;
    }
}
