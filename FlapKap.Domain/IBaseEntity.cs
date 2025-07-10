namespace FlapKap.Domain
{
    public interface IBaseEntity
    {
        DateTime AddedDate { get; set; }
        DateTime? ModifiedDate { get; set; }
        int? AddUserId { get; set; }
        int? ModifyUserId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionDate { get; set; }

    }
}