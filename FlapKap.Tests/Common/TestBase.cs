using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Headers;

namespace FlapKap.Tests.Common
{
    public class TestBase : IClassFixture<WebApplicationFactory<Program>>
    {
        protected readonly HttpClient _client;

        public TestBase(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization =
              new AuthenticationHeaderValue("Bearer",
              "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJUZXN0QnV5ZXIiLCJqdGkiOiJiZDMxYTk5YS0xOWIxLTQ2YzAtYmJlYy0wYTRkYjk2MTBlNzAiLCJ1bmlxdWVfbmFtZSI6IjEiLCJVc2VySWQiOiIxIiwicm9sZSI6IkJ1eWVyIiwibmJmIjoxNzUyMjM5MTUwLCJleHAiOjE3NTI0OTgzNTAsImlhdCI6MTc1MjIzOTE1MCwiaXNzIjoiQ29tcGFueUlzc3Vlci5jb20iLCJhdWQiOiJDb21wYW55SXNzdWVyLmNvbSJ9.9fkz4gmDA_jplJSPJh-pSZPI3zp03daUOob0brErtA0");
        }
    }
}