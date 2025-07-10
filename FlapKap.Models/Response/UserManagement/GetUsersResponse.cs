namespace FlapKap.Models.Response.UserManagement
{
    public class GetUsersResponse
    {
        public List<GeUsersResponseModel> Users { get; set; }
        public int TotalCount { get; set; }
    }
}
