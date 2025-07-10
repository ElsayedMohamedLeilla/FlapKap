using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlapKap.Domain
{
    public class BaseEntity : IBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
        public int? AddUserId { get; set; }
        public int? ModifyUserId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletionDate { get; set; }
        public void Delete()
        {
            IsDeleted = true;
            DeletionDate = DateTime.UtcNow;
        }

    }
}