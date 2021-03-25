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
        /// <param name="url">String que representa a URL.</param>
        /// <returns>A URL com caracteres válidos.</returns>
        /// <exception cref="System.ArgumentNullException">Lançado quando o valor <paramref name="url"/> for nulo.</exception>
        /// <exception cref="System.ArgumentException">Lançado quando o valor <paramref name="url"/> for uma string vazia ou de espaço em branco.</exception>
        public static string RemoveIlegalCharactersFromURL(this string url)
        {
            Guard.Against.NullOrWhiteSpace(url, nameof(url));

            foreach (var invalidChar in UrlInvalidChars)
            {
                url = url.Replace(invalidChar, string.Empty);
            }

            return url;
        }
    }
}