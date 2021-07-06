using Ardalis.GuardClauses;
using Newtonsoft.Json.Linq;
using SGP.IntegrationTests.Models;
using SGP.Shared.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SGP.IntegrationTests.Extensions
{
    public static class HttpContentExtensions
    {
        public static async Task<T> GetGraphQLDataAsync<T>(
            this HttpContent httpContent, string queryName)
        {
            Guard.Against.Null(httpContent, nameof(httpContent));
            Guard.Against.NullOrWhiteSpace(queryName, nameof(queryName));

            var response = await httpContent.ReadAsStringAsync();
            var jsonObject = JObject.Parse(response);
            var jsonString = jsonObject["data"]?[queryName]?.ToString();

            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return default;
            }

            return jsonString.FromJson<T>();
        }

        public static async Task<IEnumerable<GraphQLError>> GetGraphQLErrors(
            this HttpContent httpContent)
        {
            Guard.Against.Null(httpContent, nameof(httpContent));

            var response = await httpContent.ReadAsStringAsync();
            var jsonObject = JObject.Parse(response);
            var jsonString = jsonObject["errors"]?.ToString();

            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return Enumerable.Empty<GraphQLError>();
            }

            return jsonString.FromJson<IEnumerable<GraphQLError>>();
        }
    }
}
