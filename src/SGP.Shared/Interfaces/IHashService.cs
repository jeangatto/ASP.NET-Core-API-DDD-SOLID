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
        /// <returns>Verdadeiro se as senhas corresponderem; caso contrário, falso.</returns>
        /// <exception cref="System.ArgumentNullException">Lançado quando um ou mais argumentos tem valores nulos.</exception>
        /// <exception cref="System.ArgumentException">Lançado quando um ou mais argumentos têm valores vazios ou de espaço em branco.</exception>
        bool Compare(string text, string hash);

        /// <summary>
        /// Gera o hash de uma senha.
        /// </summary>
        /// <param name="text">A senha para criptografar.</param>
        /// <returns>A senha criptografada.</returns>
        /// <exception cref="System.ArgumentNullException">Lançado quando o valor <paramref name="text"/> for nulo.</exception>
        /// <exception cref="System.ArgumentException">Lançado quando o valor <paramref name="text"/> for uma string vazia ou de espaço em branco.</exception>
        string Hash(string text);
    }
}