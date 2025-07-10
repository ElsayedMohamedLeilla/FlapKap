namespace FlapKap.Models.DTOs
{
    public class TokenModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public IList<string> Roles { get; set; }
    }
}
