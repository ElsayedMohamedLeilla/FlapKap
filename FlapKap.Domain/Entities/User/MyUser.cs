using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlapKap.Domain.Entities.User
{
    [Table(nameof(MyUser) + "s")]
    public class MyUser : IdentityUser<int>, IBaseEntity
    {
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? AddUserId { get; set; }
        public int? ModifyUserId { get; set; }
        public decimal? Deposit { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public List<UserRole> UserRoles { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletionDate { get; set; }
        public void Delete()
        {
            IsDeleted = true;
            DeletionDate = DateTime.UtcNow;
        }
    }
}