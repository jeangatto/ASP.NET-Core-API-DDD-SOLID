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
        private static readonly IEnumerable<GraphQLError> EmptyErrors = Enumerable.Empty<GraphQLError>();

        public static async Task<T> GetGraphQLDataAsync<T>(this HttpContent httpContent, string queryName)
        {
            Guard.Against.Null(httpContent, nameof(httpContent));
            Guard.Against.NullOrWhiteSpace(queryName, nameof(queryName));

            var response = await httpContent.ReadAsStringAsync();
            var jsonObject = JObject.Parse(response);
            var dataJsonString = jsonObject["data"]?[queryName]?.ToString();

            return string.IsNullOrWhiteSpace(dataJsonString) ? default : dataJsonString.FromJson<T>();
        }

        public static async Task<IEnumerable<GraphQLError>> GetGraphQLErrors(this HttpContent httpContent)
        {
            Guard.Against.Null(httpContent, nameof(httpContent));

            var response = await httpContent.ReadAsStringAsync();
            var jsonObject = JObject.Parse(response);
            var errorsJsonString = jsonObject["errors"]?.ToString();

            return string.IsNullOrWhiteSpace(errorsJsonString)
                ? EmptyErrors
                : errorsJsonString.FromJson<IEnumerable<GraphQLError>>();
        }
    }
}