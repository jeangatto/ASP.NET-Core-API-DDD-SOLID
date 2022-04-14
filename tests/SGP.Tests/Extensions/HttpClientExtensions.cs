using System.Net.Http;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Newtonsoft.Json.Linq;
using SGP.Shared.Extensions;
using Xunit.Abstractions;

namespace SGP.Tests.Extensions;

public static class HttpClientExtensions
{
    public static async Task<TResponse> GetAsync<TResponse>(
        this HttpClient httpClient,
        ITestOutputHelper outputHelper,
        string endpoint)
    {
        Guard.Against.Null(httpClient, nameof(httpClient));
        Guard.Against.Null(outputHelper, nameof(outputHelper));
        Guard.Against.NullOrWhiteSpace(endpoint, nameof(endpoint));

        outputHelper.WriteLine($"HTTP Request: \"{endpoint}\"");
        using var httpResponseMessage = await httpClient.GetAsync(endpoint);
        return await ConvertResponseToTypeAsync<TResponse>(outputHelper, httpResponseMessage);
    }

    public static async Task<TResponse> PostAsync<TResponse>(
        this HttpClient httpClient,
        ITestOutputHelper outputHelper,
        string endpoint,
        HttpContent httpContent)
    {
        Guard.Against.Null(httpClient, nameof(httpClient));
        Guard.Against.Null(outputHelper, nameof(outputHelper));
        Guard.Against.NullOrWhiteSpace(endpoint, nameof(endpoint));
        Guard.Against.Null(httpContent, nameof(httpContent));

        outputHelper.WriteLine($"HTTP Request: \"{endpoint}\"");
        using var httpResponseMessage = await httpClient.PostAsync(endpoint, httpContent);
        return await ConvertResponseToTypeAsync<TResponse>(outputHelper, httpResponseMessage);
    }

    private static async Task<TResponse> ConvertResponseToTypeAsync<TResponse>(
        ITestOutputHelper outputHelper,
        HttpResponseMessage responseMessage)
    {
        responseMessage.EnsureSuccessStatusCode(); // Status Code 200-299
        var stringResponse = await responseMessage.Content.ReadAsStringAsync();
        outputHelper.WriteLine($"HTTP Response: \"{stringResponse}\"");
        var jObject = JObject.Parse(stringResponse);
        var jToken = jObject.SelectToken("result");
        return jToken?.HasValues == true ? jToken.ToString().FromJson<TResponse>() : default;
    }
}