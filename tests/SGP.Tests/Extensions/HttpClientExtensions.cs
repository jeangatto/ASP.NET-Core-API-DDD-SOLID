using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Server;
using Newtonsoft.Json.Linq;
using SGP.Shared.Extensions;
using SGP.Tests.Models;
using Throw;
using Xunit.Abstractions;

namespace SGP.Tests.Extensions
{
    public static class HttpClientExtensions
    {
        private static readonly IEnumerable<GraphQLError> EmptyErrors = Enumerable.Empty<GraphQLError>();

        public static async Task<T> SendAndDeserializeAsync<T>(
            this HttpClient httpClient,
            ITestOutputHelper outputHelper,
            string endpoint)
        {
            httpClient.ThrowIfNull();
            outputHelper.ThrowIfNull();
            endpoint.ThrowIfNull().IfEmpty().IfWhiteSpace();

            outputHelper.WriteLine($"HTTP Request: \"{endpoint}\"");

            using var httpResponseMessage = await httpClient.GetAsync(endpoint);
            httpResponseMessage.EnsureSuccessStatusCode(); // Status Code 200-299
            var stringResponse = await httpResponseMessage.Content.ReadAsStringAsync();
            outputHelper.WriteLine($"HTTP Response: {stringResponse}");

            var jObject = JObject.Parse(stringResponse);
            var jToken = jObject.SelectToken("result");
            return jToken?.HasValues == true ? jToken.ToString().FromJson<T>() : default;
        }

        public static async Task<T> SendAndDeserializeAsync<T>(
            this HttpClient httpClient,
            ITestOutputHelper outputHelper,
            string endpoint,
            GraphQLRequest request,
            string fieldName)
        {
            httpClient.ThrowIfNull();
            outputHelper.ThrowIfNull();
            endpoint.ThrowIfNull().IfEmpty().IfWhiteSpace();
            request.ThrowIfNull();
            fieldName.ThrowIfNull().IfEmpty().IfWhiteSpace();

            var requestBody = request.ToJson();
            outputHelper.WriteLine($"HTTP Request: \"{endpoint}\", Body: {requestBody}");

            using var httpContent = new StringContent(requestBody, Encoding.UTF8, MediaTypeNames.Application.Json);
            using var httpResponseMessage = await httpClient.PostAsync(endpoint, httpContent);
            httpResponseMessage.EnsureSuccessStatusCode(); // Status Code 200-299
            var stringResponse = await httpResponseMessage.Content.ReadAsStringAsync();
            outputHelper.WriteLine($"HTTP Response: {stringResponse}");

            var jObject = JObject.Parse(stringResponse);
            var jToken = jObject.SelectToken($"data.{fieldName}");
            return jToken?.HasValues == true ? jToken.ToString().FromJson<T>() : default;
        }

        public static async Task<IEnumerable<GraphQLError>> SendAndGetErrorsAsync(
            this HttpClient httpClient,
            ITestOutputHelper outputHelper,
            string endpoint,
            GraphQLRequest request)
        {
            httpClient.ThrowIfNull();
            outputHelper.ThrowIfNull();
            endpoint.ThrowIfNull().IfEmpty().IfWhiteSpace();
            request.ThrowIfNull();

            var requestBody = request.ToJson();
            outputHelper.WriteLine($"HTTP Request: \"{endpoint}\", Body: {requestBody}");

            using var httpContent = new StringContent(requestBody, Encoding.UTF8, MediaTypeNames.Application.Json);
            using var httpResponseMessage = await httpClient.PostAsync(endpoint, httpContent);
            httpResponseMessage.EnsureSuccessStatusCode(); // Status Code 200-299
            var stringResponse = await httpResponseMessage.Content.ReadAsStringAsync();
            outputHelper.WriteLine($"HTTP Response: {stringResponse}");

            var jObject = JObject.Parse(stringResponse);
            var jToken = jObject.SelectToken("errors");
            return jToken?.HasValues == true ? jToken.ToString().FromJson<IEnumerable<GraphQLError>>() : EmptyErrors;
        }
    }
}