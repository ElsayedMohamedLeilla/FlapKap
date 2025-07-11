using FlapKap.Models.DTOs.Users;
using FlapKap.Models.Response;
using FlapKap.Models.Response.UserManagement;
using FlapKap.Tests.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace FlapKap.Tests.Controllers
{
    public class UserControllerTests(WebApplicationFactory<Program> factory) : TestBase(factory)
    {
        private readonly DateTime timeNow = DateTime.UtcNow;

        [Fact]
        public async Task CreateUser_ReturnsOk()
        {
            var product = new CreateUserModel
            {
                UserName = "UserTest" + timeNow.Ticks,
                Password = "123456",
                Role = UserRoleEnum.Seller
            };

            var responseCreate = await _client.PostAsJsonAsync("/api/User/Create", product);
            Assert.Equal(HttpStatusCode.OK, responseCreate.StatusCode);
            var contentCreate = await responseCreate.Content.ReadAsStringAsync();
            var resultCreate = JsonSerializer.Deserialize<SuccessResponse<int>>(contentCreate, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.True(resultCreate != null && resultCreate.Data > 0);

        }
        [Fact]
        public async Task UpdateUser_ReturnsOk()
        {
            #region Get Data

            var responseGet = await _client.GetAsync("/api/User/Get?PagingEnabled=true&PageSize=5&PageNumber=0");
            responseGet.EnsureSuccessStatusCode();
            var contentGet = await responseGet.Content.ReadAsStringAsync();
            var resultGet = JsonSerializer.Deserialize<SuccessResponse<List<GeUsersResponseModel>>>(contentGet, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var Id = resultGet.Data.First().Id;

            #endregion

            var product = new UpdateUserModel
            {
                Id = Id,
                UserName = "UserTestUpdated" + timeNow.Ticks,
                Role = UserRoleEnum.Seller
            };

            var responseUpdate = await _client.PutAsJsonAsync("/api/User/Update", product);

            Assert.Equal(HttpStatusCode.OK, responseUpdate.StatusCode);

            var contentUpdate = await responseUpdate.Content.ReadAsStringAsync();
            var resultUpdate = JsonSerializer.Deserialize<SuccessResponse<bool>>(contentUpdate, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.True(resultUpdate != null && resultUpdate.Data);

        }
        [Fact]
        public async Task GetUsers_ReturnsOk()
        {
            var responseGet = await _client.GetAsync("/api/User/Get?PagingEnabled=true&PageSize=5&PageNumber=0");
            responseGet.EnsureSuccessStatusCode();
            var contentGet = await responseGet.Content.ReadAsStringAsync();
            var resultGet = JsonSerializer.Deserialize<SuccessResponse<List<GeUsersResponseModel>>>(contentGet, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.True(resultGet.Data != null && resultGet.Data.Count > 0);
        }
        [Fact]
        public async Task GetUserById_ReturnsOk()
        {
            #region Get Data

            var responseGet = await _client.GetAsync("/api/User/Get?PagingEnabled=true&PageSize=5&PageNumber=0");
            responseGet.EnsureSuccessStatusCode();
            var contentGet = await responseGet.Content.ReadAsStringAsync();
            var resultGet = JsonSerializer.Deserialize<SuccessResponse<List<GeUsersResponseModel>>>(contentGet, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var Id = resultGet.Data.First().Id;

            #endregion

            var responseGetById = await _client.GetAsync("/api/User/GetById?userId=" + Id);
            responseGetById.EnsureSuccessStatusCode();
            var contentGetById = await responseGetById.Content.ReadAsStringAsync();
            var resultGetById = JsonSerializer.Deserialize<SuccessResponse<GetUserByIdResponseModel>>(contentGetById, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.True(resultGetById.Data != null && resultGetById.Data.Id > 0);
        }
        [Fact]
        public async Task DeleteUser_ReturnsOk()
        {
            #region Get Data

            var responseGet = await _client.GetAsync("/api/User/Get?PagingEnabled=true&PageSize=5&PageNumber=0");
            responseGet.EnsureSuccessStatusCode();
            var contentGet = await responseGet.Content.ReadAsStringAsync();
            var resultGet = JsonSerializer.Deserialize<SuccessResponse<List<GeUsersResponseModel>>>(contentGet, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var Id = resultGet.Data.First().Id;

            #endregion

            var responseGetById = await _client.DeleteAsync("/api/User/Delete?userId=" + Id);
            responseGetById.EnsureSuccessStatusCode();
            var contentGetById = await responseGetById.Content.ReadAsStringAsync();
            var resultGetById = JsonSerializer.Deserialize<SuccessResponse<bool>>(contentGetById, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.True(resultGetById.Data);
        }
    }
}
