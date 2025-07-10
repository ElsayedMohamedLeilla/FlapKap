namespace FlapKap.Models.DTOs.Users
{
    public class CreateUserModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public UserRoleEnum Role { get; set; }
    }
}
