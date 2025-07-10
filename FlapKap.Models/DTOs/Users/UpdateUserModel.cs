namespace FlapKap.Models.DTOs.Users
{
    public class UpdateUserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public UserRoleEnum Role { get; set; }
    }
}
