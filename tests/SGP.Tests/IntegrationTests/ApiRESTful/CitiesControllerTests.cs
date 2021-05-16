using FluentAssertions;
using FluentResults;
using FluentResults.Extensions.FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using SGP.Application.Responses;
using SGP.Shared.Extensions;
using SGP.Tests.Factories;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.IntegrationTests.ApiRESTful
{
    [Category(TestCategories.ApiRESTful)]
    public class CitiesControllerTests : IClassFixture<WebTestApplicationFactory>
    {
        private const string EndPoint = "/api/cities";
        private readonly HttpClient _httpClient;

        public CitiesControllerTests(WebTestApplicationFactory factory)
        {
            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost")
            });
        }

        [Fact]
        public async Task Get_AllStates_ReturnsOkResult()
        {
            // Arrange
            const int expectedCount = 27;

            // Act
            var response = await _httpClient.GetAsync($"{EndPoint}/states");

            // Assert
            response.EnsureSuccessStatusCode();

            var states = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
            states.Should().NotBeEmpty()
                .And.OnlyHaveUniqueItems()
                .And.BeInAscendingOrder()
                .And.HaveCount(expectedCount);
        }

        [Fact]
        public async Task Get_ExistingStateAbbr_ReturnsOkResult()
        {
            // Arrange
            const int expectedCount = 645;
            const string stateAbbr = "SP";

            // Act
            var response = await _httpClient.GetAsync($"{EndPoint}/{stateAbbr}");

            // Assert
            response.EnsureSuccessStatusCode();

            var cities = await response.Content.ReadFromJsonAsync<IEnumerable<CityResponse>>();
            cities.Should().NotBeEmpty()
                .And.OnlyHaveUniqueItems()
                .And.HaveCount(expectedCount);
        }

        [Theory]
        [InlineData("AK", HttpStatusCode.NotFound)]     // AK: Alasca
        [InlineData("WA", HttpStatusCode.NotFound)]     // WA: Washington
        [InlineData("SantanaParnaiba", HttpStatusCode.BadRequest)]  // Máximo de 2 caracteres
        public async Task Get_NonExistingOrInvalidStateAbbr_Returns(string stateAbbr, HttpStatusCode statusCode)
        {
            // Act
            var response = await _httpClient.GetAsync($"{EndPoint}/{stateAbbr}");

            // Assert
            response.StatusCode.Should().Be(statusCode);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrWhiteSpace();

            var errors = content.FromJson<IEnumerable<Error>>();
            errors.Should().NotBeEmpty().And.OnlyHaveUniqueItems();
        }
    }
}