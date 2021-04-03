namespace SGP.Shared.Results
{
    public partial class Result
    {
        /// <summary>
        /// Cria um resultado de sucesso.
        /// </summary>
        public static Result Success() => new(false);

        /// <summary>
        /// Cria um resultado de sucesso contendo o valor fornecido.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static Result<T> Success<T>() => new(false);

        /// <summary>
        /// Cria um resultado de sucesso contendo o valor fornecido.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">O valor do resultado.</param>
        public static Result<T> Success<T>(T value) => new(false, value);
    }
}
