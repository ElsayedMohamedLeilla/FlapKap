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
              new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJUZXN0VXBkYXRlZCIsImp0aSI6IjQ5MTFlOGJkLWY3YmItNDc0MC05ZjJhLTk2MmJjMjdkYzc0ZCIsInVuaXF1ZV9uYW1lIjoiMSIsIlVzZXJJZCI6IjEiLCJyb2xlIjoiU2VsbGVyIiwibmJmIjoxNzUyMTg2MjA2LCJleHAiOjE3NTI0NDU0MDYsImlhdCI6MTc1MjE4NjIwNiwiaXNzIjoiQ29tcGFueUlzc3Vlci5jb20iLCJhdWQiOiJDb21wYW55SXNzdWVyLmNvbSJ9.P66kgFpLCaXzd9ODEr3NDS72u6l1Y9ZG4ZuL6tYuMl8");
        }
    }
}