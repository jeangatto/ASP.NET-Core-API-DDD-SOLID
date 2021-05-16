using FluentAssertions;
using SGP.Tests.Extensions;
using SGP.Tests.Factories;
using SGP.Tests.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.IntegrationTests.GraphQL
{
    [Category(TestCategories.GraphQL)]
    public class CitySchemaTests : IClassFixture<WebTestApplicationFactory>
    {
        private const string GraphQLEndPoint = "/graphql/cities";
        private readonly HttpClient _httpClient;

        public CitySchemaTests(WebTestApplicationFactory factory)
        {
            factory.Server.AllowSynchronousIO = true;
            factory.Server.BaseAddress = new Uri("https://localhost");

            _httpClient = factory.Server.CreateClient();
        }

        [Fact]
        public async Task Should_GetAllStatesQuery_ReturnsStates()
        {
            // Arrange
            const int expectedCount = 27;
            const string query = @"{ ""query"" :
                        ""query getAllStates { states }"",
                        ""operationName"" : ""getAllStates""
                    }";

            // Act
            var response = await _httpClient.PostAsync(GraphQLEndPoint, query.CreateHttpContent());

            // Assert
            response.EnsureSuccessStatusCode();

            var graphQLResponse = await response.Content.ReadFromJsonAsync<GraphQLResponse<StatesResponse>>();
            graphQLResponse.Data.States.Should().NotBeEmpty()
                .And.OnlyHaveUniqueItems()
                .And.BeInAscendingOrder()
                .And.HaveCount(expectedCount);
        }

        internal class StatesResponse
        {
            public StatesResponse(IEnumerable<string> states)
            {
                States = states;
            }

            public IEnumerable<string> States { get; }
        }
    }
}