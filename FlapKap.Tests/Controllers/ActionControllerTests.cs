using FlapKap.Models.DTOs.UserActions;
using FlapKap.Models.Response;
using FlapKap.Models.Response.Products;
using FlapKap.Models.Response.UserManagement;
using FlapKap.Tests.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace FlapKap.Tests.Controllers
{
    public class ActionControllerTests(WebApplicationFactory<Program> factory) : TestBase(factory)
    {
        private readonly DateTime timeNow = DateTime.UtcNow;

        [Fact]
        public async Task DepositAction_ReturnsOk()
        {
            var model = new DepositModel
            {
                DepositAmount = 100
            };

            var responseCreate = await _client.PutAsJsonAsync("/api/Action/Deposit", model);
            Assert.Equal(HttpStatusCode.OK, responseCreate.StatusCode);
            var contentCreate = await responseCreate.Content.ReadAsStringAsync();
            var resultCreate = JsonSerializer.Deserialize<SuccessResponse<bool>>(contentCreate, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.True(resultCreate != null && resultCreate.Data);

        }
        [Fact]
        public async Task ResetAction_ReturnsOk()
        {
            var responseCreate = await _client.PutAsJsonAsync("/api/Action/Reset", new StringContent(string.Empty));
            Assert.Equal(HttpStatusCode.OK, responseCreate.StatusCode);
            var contentCreate = await responseCreate.Content.ReadAsStringAsync();
            var resultCreate = JsonSerializer.Deserialize<SuccessResponse<bool>>(contentCreate, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.True(resultCreate != null && resultCreate.Data);
        }
        [Fact]
        public async Task BuyAction_ReturnsOk()
        {
            var model = new BuyModel();

            #region Get Data

            var responseGet = await _client.GetAsync("/api/Product/Get?PagingEnabled=true&PageSize=5&PageNumber=0");
            responseGet.EnsureSuccessStatusCode();
            var contentGet = await responseGet.Content.ReadAsStringAsync();
            var resultGet = JsonSerializer.Deserialize<SuccessResponse<List<GetProductsResponseModel>>>(contentGet, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var products = resultGet.Data;

            #endregion

            foreach (var item in products)
            {
                if (item.Quantity > 0)
                {
                    model.Items.Add(new BuyItemModel
                    {
                        ProductId = item.Id,
                        Quantity = 1
                    });
                }
            }

            var responseBuy = await _client.PutAsJsonAsync("/api/Action/Buy", model);

            Assert.Equal(HttpStatusCode.OK, responseBuy.StatusCode);

            var contentBuy = await responseBuy.Content.ReadAsStringAsync();
            var resultBuy = JsonSerializer.Deserialize<SuccessResponse<BuyResponseModel>>(contentBuy, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.True(resultBuy != null && resultBuy.Data.Products.Count > 0);

        }
    }
}
