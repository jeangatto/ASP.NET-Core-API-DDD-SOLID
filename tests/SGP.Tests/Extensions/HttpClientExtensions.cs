using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using GraphQL.Server;
using SGP.Shared.Extensions;

namespace SGP.Tests.Extensions
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, string endpoint,
            GraphQLRequest request)
        {
            Guard.Against.Null(httpClient, nameof(httpClient));
            Guard.Against.NullOrWhiteSpace(endpoint, nameof(endpoint));
            Guard.Against.Null(request, nameof(request));

            var httpContent = new StringContent(
                request.ToJson(),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

            return httpClient.PostAsync(endpoint, httpContent);
        }
    }
}