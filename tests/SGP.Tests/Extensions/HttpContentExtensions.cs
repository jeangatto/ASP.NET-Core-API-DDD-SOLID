using Ardalis.GuardClauses;
using Newtonsoft.Json.Linq;
using SGP.Shared.Extensions;
using SGP.Tests.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SGP.Tests.Extensions
{
    public static class HttpContentExtensions
    {
        public static async Task<T> GetGraphQLDataAsync<T>(this HttpContent httpContent, string queryName)
        {
            Guard.Against.Null(httpContent, nameof(httpContent));
            Guard.Against.NullOrWhiteSpace(queryName, nameof(queryName));

            var response = await httpContent.ReadAsStringAsync();
            var jsonObject = JObject.Parse(response);
            var jsonString = jsonObject["data"]?[queryName]?.ToString();

            return string.IsNullOrWhiteSpace(jsonString) ? default : jsonString.FromJson<T>();
        }

        public static async Task<IEnumerable<GraphQLError>> GetGraphQLErrors(this HttpContent httpContent)
        {
            Guard.Against.Null(httpContent, nameof(httpContent));

            var response = await httpContent.ReadAsStringAsync();
            var jsonObject = JObject.Parse(response);
            var jsonString = jsonObject["errors"]?.ToString();

            return string.IsNullOrWhiteSpace(jsonString) ?
                Enumerable.Empty<GraphQLError>() : jsonString.FromJson<IEnumerable<GraphQLError>>();
        }
    }
}
