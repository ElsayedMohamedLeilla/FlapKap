using Microsoft.AspNetCore.Identity;

namespace FlapKap.Domain.Entities.User
{
    public class UserRole : IdentityUserRole<int>
    {
        public virtual MyUser User { get; set; }
        public virtual Role Role { get; set; }
    }
}