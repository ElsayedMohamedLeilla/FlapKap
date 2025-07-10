using FlapKap.Models.DTOs.Users;

namespace FlapKap.Models.Response.UserManagement
{
    public class GetUserByIdResponseModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public UserRoleEnum Role { get; set; }
    }
}
