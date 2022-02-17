using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using GraphQL.Server;
using SGP.Shared.Extensions;
using Xunit.Abstractions;

namespace SGP.Tests.Extensions
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> SendAsync(
            this HttpClient httpClient,
            ITestOutputHelper output,
            string endpoint,
            GraphQLRequest request)
        {
            Guard.Against.Null(httpClient, nameof(httpClient));
            Guard.Against.Null(output, nameof(output));
            Guard.Against.NullOrWhiteSpace(endpoint, nameof(endpoint));
            Guard.Against.Null(request, nameof(request));

            var requestBody = request.ToJson();
            output.WriteLine($"HTTP Request: \"{endpoint}\", Body: {requestBody}");

            using var httpContent = new StringContent(requestBody, Encoding.UTF8, MediaTypeNames.Application.Json);
            return httpClient.PostAsync(endpoint, httpContent);
        }
    }
}