using SGP.Shared.Extensions;
using System.Net.Http;
using System.Net.Mime;
using System.Text;

namespace SGP.Tests.Extensions
{
    public static class HttpContentExtensions
    {
        public static StringContent CreateHttpContent(this string query)
        {
            return new StringContent(query, Encoding.UTF8, MediaTypeNames.Application.Json);
        }

        public static StringContent CreateHttpContent<T>(this T data)
        {
            return new StringContent(data.ToJson(), Encoding.UTF8, MediaTypeNames.Application.Json);
        }
    }
}
