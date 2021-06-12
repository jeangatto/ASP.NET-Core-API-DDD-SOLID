using Ardalis.GuardClauses;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using System;
using System.Net.Http;

namespace SGP.SharedTests.GraphQL
{
    public static class GraphQLClient
    {
        private static readonly NewtonsoftJsonSerializer DefaultSerializer = new();

        /// <summary>
        /// Cria uma nova instância do <see cref="GraphQLHttpClient"/> com o Serializador configurado.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="endPoint">O EndPoint do GraphQL a ser consumido.</param>
        /// <returns>O Cliente Http do GraphQL.</returns>
        public static IGraphQLClient Create(HttpClient httpClient, string endPoint)
        {
            Guard.Against.Null(httpClient, nameof(httpClient));
            Guard.Against.NullOrWhiteSpace(endPoint, nameof(endPoint));

            var options = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(httpClient.BaseAddress, endPoint)
            };

            return new GraphQLHttpClient(options, DefaultSerializer, httpClient);
        }
    }
}