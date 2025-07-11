namespace FlapKap.Models.DTOs.Products
{
    public class UpdateProductModel
    {
        public int Id { get; set; }
        public decimal Cost { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
