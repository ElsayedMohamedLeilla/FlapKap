namespace FlapKap.Models.Response.Products
{
    public class GetProductByIdResponseModel
    {
        public int Id { get; set; }
        public decimal Cost { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
