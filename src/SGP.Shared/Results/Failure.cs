namespace SGP.Shared.Results
{
    public partial class Result
    {
        /// <summary>
        /// Cria um resultado de falha.
        /// </summary>
        public static Result Failure() => new(true);

        /// <summary>
        /// Cria um resultado de falha com a mensagem de erro fornecida.
        /// </summary>
        /// <param name="error">Mensagem de erro.</param>
        public static Result Failure(string error) => new(true, error);

        /// <summary>
        /// Cria um resultado de falha com a mensagem de erro fornecida.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="error">Mensagem de erro.</param>
        public static Result<T> Failure<T>(string error) => new(true, error);
    }
}