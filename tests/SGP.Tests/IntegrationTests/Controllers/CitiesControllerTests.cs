using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using SGP.Application.Responses;
using SGP.Tests.Factories;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace SGP.Tests.IntegrationTests.Controllers
{
    [Category(TestCategories.ApiRESTful)]
    public class CitiesControllerTests : IClassFixture<WebApiApplicationFactory>
    {
        private readonly HttpClient _httpClient;

        public CitiesControllerTests(WebApiApplicationFactory factory)
        {
            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Get_ExistingStateAbbr_ReturnsOkResult()
        {
            // Arrange
            const int expectedCount = 645;
            const string stateAbbr = "SP";

            // Act
            var response = await _httpClient.GetAsync($"/api/cities/{stateAbbr}");

            // Assert
            response.EnsureSuccessStatusCode();
            var cities = await response.Content.ReadFromJsonAsync<IEnumerable<CityResponse>>();
            cities.Should().NotBeEmpty()
                .And.OnlyHaveUniqueItems()
                .And.HaveCount(expectedCount);
        }

        [Fact]
        public async Task Get_AllStates_ReturnsOkResult()
        {
            // Arrange
            const int expectedCount = 27;

            // Act
            var response = await _httpClient.GetAsync("/api/cities/states/");

            // Assert
            response.EnsureSuccessStatusCode();
            var states = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
            states.Should().NotBeEmpty()
                .And.OnlyHaveUniqueItems()
                .And.BeInAscendingOrder()
                .And.HaveCount(expectedCount);
        }
    }
}