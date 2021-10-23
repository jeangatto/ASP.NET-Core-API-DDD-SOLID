using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Newtonsoft.Json.Linq;
using SGP.Shared.Extensions;
using SGP.Tests.Models;

namespace SGP.Tests.Extensions
{
    public static class HttpContentExtensions
    {
        private static readonly IEnumerable<GraphError> EmptyErrors = Enumerable.Empty<GraphError>();

        public static async Task<T> GetDataAsync<T>(this HttpContent httpContent, string queryName)
        {
            Guard.Against.Null(httpContent, nameof(httpContent));
            Guard.Against.NullOrWhiteSpace(queryName, nameof(queryName));

            var response = await httpContent.ReadAsStringAsync();
            var jsonObject = JObject.Parse(response);
            var dataStrJson = jsonObject["data"]?[queryName]?.ToString();

            return string.IsNullOrWhiteSpace(dataStrJson) ? default : dataStrJson.FromJson<T>();
        }

        public static async Task<IEnumerable<GraphError>> GetErrorsAsync(this HttpContent httpContent)
        {
            Guard.Against.Null(httpContent, nameof(httpContent));

            var response = await httpContent.ReadAsStringAsync();
            var jsonObject = JObject.Parse(response);
            var errorsStrJson = jsonObject["errors"]?.ToString();

            return string.IsNullOrWhiteSpace(errorsStrJson)
                ? EmptyErrors
                : errorsStrJson.FromJson<IEnumerable<GraphError>>();
        }
    }
}