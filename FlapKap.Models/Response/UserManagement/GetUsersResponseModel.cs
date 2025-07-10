using FlapKap.Models.Response;

namespace FlapKap.Models.Response.UserManagement
{
    public class GeUsersResponseModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public decimal? Deposit { get; set; }
    }
}
