namespace FlapKap.Models.Response.UserManagement
{
    public class GetBuyValidationResponseModel
    {
        public decimal AllItemsCost { get; set; }
        public List<BuyValidationItemModel> DBItems { get; set; }
    }
    public class BuyValidationItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
    }
}
