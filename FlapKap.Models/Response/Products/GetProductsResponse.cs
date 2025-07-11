namespace FlapKap.Models.Response.Products
{
    public class GetProductsResponse
    {
        public List<GetProductsResponseModel> Products { get; set; }
        public int TotalCount { get; set; }
    }
}
