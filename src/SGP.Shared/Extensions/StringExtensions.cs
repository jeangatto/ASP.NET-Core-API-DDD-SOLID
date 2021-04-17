using Ardalis.GuardClauses;

namespace SGP.Shared.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Lista com caracteres reservados e ilegais.
        /// </summary>
        private static readonly string[] UrlInvalidChars = new[] { "{", "}", "|", @"\", "^", "[", "]", "`", ";", "/", "?", ":", "@", "&", "=", "+", "$", "," };

        /// <summary>
        /// Remove da string caracteres reservados e ilegais para URL.
        /// REF: https://stackoverflow.com/a/13500078/4494758
        /// </summary>
        /// <param name="uriString">String que representa a URL.</param>
        /// <returns>A URL com caracteres válidos.</returns>
        /// <exception cref="System.ArgumentNullException">Lançado quando o valor <paramref name="uriString"/> for nulo.</exception>
        /// <exception cref="System.ArgumentException">Lançado quando o valor <paramref name="uriString"/> for uma string vazia ou de espaço em branco.</exception>
        public static string RemoveIlegalCharactersFromURL(this string uriString)
        {
            Guard.Against.NullOrWhiteSpace(uriString, nameof(uriString));

            foreach (var invalidChar in UrlInvalidChars)
            {
                uriString = uriString.Replace(invalidChar, string.Empty);
            }

            return uriString;
        }
    }
}