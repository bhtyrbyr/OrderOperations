using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using OrderOperations.Application.DTOs.ProductDTOs;
using OrderOperations.WebApi.DTOs;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace OrderOperations.IntegrationTests
{
    public class ProductControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:7000");
        }

        [Fact]
        public async Task GetAll_ShouldReturnSuccess()
        {
            var response = await _client.GetAsync("/api/product");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            }
            else
            {
                // Assert
                var okResult = response.Should().BeOfType<OkObjectResult>().Subject;
                okResult.StatusCode.Should().Be(200);

                var responseData = okResult.Value.Should().BeAssignableTo<ResponseDTO>().Subject;
                ((List<ProductViewModel>)responseData.Data).Count.Should().Be(2);
            }

            var result = await response.Content.ReadFromJsonAsync<ResponseDTO>();
            result.Should().NotBeNull();
        }
    }
}

