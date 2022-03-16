using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Server;
using SGP.Shared.Extensions;
using Throw;
using Xunit.Abstractions;

namespace SGP.Tests.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> SendAsync(
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
            return await httpClient.PostAsync(endpoint, httpContent);
        }
    }
}