namespace FlapKap.Models.DTOs.Users
{
    public class UserDTO
    {
        public UserDTO()
        {
        }
        public int Id { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? AddUserId { get; set; }
        public int? ModifyUserId { get; set; }
        public int BranchId { get; set; }
        public virtual string UserName { get; set; }
        public bool IsActive { get; set; }
        public int MobileCountryId { get; set; }
        public string MobileNumber { get; set; }
        public virtual string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string Name { get; set; }
        public string Password { get; set; }

    }


}
