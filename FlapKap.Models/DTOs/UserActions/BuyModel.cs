namespace FlapKap.Models.DTOs.UserActions
{
    public class BuyModel
    {
        public BuyModel()
        {
            Items = [];
        }
        public List<BuyItemModel> Items { get; set; }
    }
}
