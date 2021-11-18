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

        public static async Task<T> GetGraphDataAsync<T>(this HttpContent httpContent, string fieldName)
        {
            Guard.Against.Null(httpContent, nameof(httpContent));
            Guard.Against.NullOrWhiteSpace(fieldName, nameof(fieldName));

            var response = await httpContent.ReadAsStringAsync();
            var jObject = JObject.Parse(response);
            var jToken = jObject.SelectToken($"data.{fieldName}");
            return jToken?.HasValues == true ? jToken.ToString().FromJson<T>() : default;
        }

        public static async Task<IEnumerable<GraphError>> GetGraphErrorsAsync(this HttpContent httpContent)
        {
            Guard.Against.Null(httpContent, nameof(httpContent));

            var response = await httpContent.ReadAsStringAsync();
            var jObject = JObject.Parse(response);
            var jToken = jObject.SelectToken("errors");
            return jToken?.HasValues == true ? jToken.ToString().FromJson<IEnumerable<GraphError>>() : EmptyErrors;
        }
    }
}