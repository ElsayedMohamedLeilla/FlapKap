namespace FlapKap.Models.Response.UserManagement
{
    public class GetProductsResponse
    {
        public List<GetProductsResponseModel> Products { get; set; }
        public int TotalCount { get; set; }
    }
}
