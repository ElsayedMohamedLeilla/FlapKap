using FlapKap.Domain.Entities.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlapKap.Domain.Entities.Product
{
    [Table(nameof(Product) + "s")]
    public class Product : BaseEntity
    {
        #region Forign Key
        public int SellerId { get; set; }
        [ForeignKey(nameof(SellerId))]
        public MyUser Seller { get; set; }
        #endregion

        public decimal Cost { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
