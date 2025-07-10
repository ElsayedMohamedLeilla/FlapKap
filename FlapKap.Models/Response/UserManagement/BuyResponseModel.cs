namespace FlapKap.Models.Response.UserManagement
{
    public class BuyResponseModel
    {
        public BuyResponseModel()
        {
            ChangeList = [];
            Products = [];
        }
        public decimal TotalSpent { get; set; }
        public List<BuyResponseItemModel> Products { get; set; }
        public decimal TotalRemaining { get; set; }
        public List<decimal> ChangeList { get; set; }
    }
    public class BuyResponseItemModel
    {
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
