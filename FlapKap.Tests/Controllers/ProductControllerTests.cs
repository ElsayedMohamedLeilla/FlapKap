using FlapKap.Models.DTOs.Products;
using FlapKap.Models.DTOs.Users;
using FlapKap.Models.Response;
using FlapKap.Models.Response.Products;
using FlapKap.Tests.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace FlapKap.Tests.Controllers
{
    public class ProductControllerTests : TestBase
    {
        DateTime timeNow = DateTime.UtcNow;
        public ProductControllerTests(WebApplicationFactory<Program> factory) :
            base(factory)
        { }

        [Fact]
        public async Task AddProduct_WithValidData_ReturnsCreatedProduct()
        {
            // Arrange
            var product = new CreateProductModel
            {
                Name = "Test Product 12" + timeNow,
                Cost = 99.99m,
                Quantity = 47
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Product/Create", product);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<SuccessResponse<int>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.True(result != null && result.Data > 0);
        }
        [Fact]
        public async Task GetProducts_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/Product/Get?PagingEnabled=true&PageSize=5&PageNumber=0");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<SuccessResponse<List<GetProductsResponseModel>>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.True(result.Data != null && result.Data.Count > 0);
        }
    }
}
