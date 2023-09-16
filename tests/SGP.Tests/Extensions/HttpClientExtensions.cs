using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SGP.Shared.Extensions;

namespace SGP.Tests.Extensions;

internal static class HttpClientExtensions
{
    public static async Task<TResponse> GetAsync<TResponse>(
        this HttpClient httpClient,
        string endpoint)
    {
        using var httpResponse = await httpClient.GetAsync(endpoint);
        return await ConvertResponseToTypeAsync<TResponse>(httpResponse);
    }

    public static async Task<TResponse> PostAsync<TResponse>(
        this HttpClient httpClient,
        string endpoint,
        HttpContent httpContent)
    {
        using var httpResponse = await httpClient.PostAsync(endpoint, httpContent);
        return await ConvertResponseToTypeAsync<TResponse>(httpResponse);
    }

    private static async Task<TResponse> ConvertResponseToTypeAsync<TResponse>(HttpResponseMessage httpResponse)
    {
        httpResponse.EnsureSuccessStatusCode(); // Status Code 200-299

        var response = await httpResponse.Content.ReadAsStringAsync();
        var jObject = JObject.Parse(response);
        var jToken = jObject.SelectToken("result", errorWhenNoMatch: false);
        return jToken?.HasValues == true ? jToken.ToString().FromJson<TResponse>() : default;
    }
}