using Ardalis.GuardClauses;
using Newtonsoft.Json.Linq;
using SGP.Shared.Extensions;
using System.Net.Http;
using System.Threading.Tasks;

namespace SGP.SharedTests.Extensions
{
    public static class HttpContentExtensions
    {
        public static async Task<T> GetGraphQLDataAsync<T>(this HttpContent httpContent,
            string queryName)
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
    }
}
