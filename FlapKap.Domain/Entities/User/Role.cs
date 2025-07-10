using Microsoft.AspNetCore.Identity;

namespace FlapKap.Domain.Entities.User
{
    public class Role : IdentityRole<int>
    {
        public List<UserRole> UserRoles { get; set; }
    }
}