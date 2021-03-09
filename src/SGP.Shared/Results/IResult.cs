namespace SGP.Shared.Results
{
    public interface IResult
    {
        /// <summary>
        /// Mensagem do resultado (opcional).
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Indica se o resultado foi bem-sucedido.
        /// </summary>
        bool Succeeded { get; }
    }

    /// <summary>
    /// Resultado com dados.
    /// </summary>
    /// <typeparam name="T">O tipo do objeto.</typeparam>
    public interface IResult<out T> : IResult
    {
        /// <summary>
        /// Dados do resultado.
        /// </summary>
        T Data { get; }
    }
}