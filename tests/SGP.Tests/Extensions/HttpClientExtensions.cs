using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SGP.Shared.Extensions;
using Throw;
using Xunit.Abstractions;

namespace SGP.Tests.Extensions;

public static class HttpClientExtensions
{
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
}