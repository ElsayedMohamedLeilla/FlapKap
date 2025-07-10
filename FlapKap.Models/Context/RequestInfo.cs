using FlapKap.Domain.Entities.User;
using FlapKap.Models.DTOs.Users;

namespace FlapKap.Models.Context
{
    public class RequestInfo
    {
        public MyUser User { get; set; }
        public UserRoleEnum UserRole { get; set; }
        public string Lang { get; set; }
        public int UserId { get; set; }
    }
}
