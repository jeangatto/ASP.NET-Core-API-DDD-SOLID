using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using System;
using System.Net.Http;

namespace SGP.Tests.GraphQL
{
    public static class GraphQLClient
    {
        private static readonly NewtonsoftJsonSerializer DefaultSerializer = new();

        public static IGraphQLClient Create(HttpClient httpClient, string graphqlEndPoint)
        {
            return new GraphQLHttpClient(new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(httpClient.BaseAddress, graphqlEndPoint)
            }, DefaultSerializer, httpClient);
        }
    }
}
