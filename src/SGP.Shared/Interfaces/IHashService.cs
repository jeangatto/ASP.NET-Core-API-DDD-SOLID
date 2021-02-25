namespace SGP.Shared.Interfaces
{
    /// <summary>
    /// Criptografia de via única.
    /// </summary>
    public interface IHashService
    {
        /// <summary>
        /// Verifica se o hash do texto corresponde ao hash fornecido.
        /// </summary>
        /// <param name="text">O texto para verificar.</param>
        /// <param name="hash">A senha criptografada anteriormente.</param>
        /// <exception cref="System.ArgumentException">Lançado quando um ou mais argumentos têm valores não suportados ou ilegais.</exception>
        /// <returns>Verdadeiro se as senhas corresponderem; caso contrário, falso.</returns>
        bool Compare(string text, string hash);

        /// <summary>
        /// Gera o hash de uma senha.
        /// </summary>
        /// <param name="text">A senha para criptografar.</param>
        /// <exception cref="System.ArgumentException">Lançado quando um ou mais argumentos têm valores não suportados ou ilegais.</exception>
        /// <returns>A senha com criptografada.</returns>
        string Hash(string text);
    }
}