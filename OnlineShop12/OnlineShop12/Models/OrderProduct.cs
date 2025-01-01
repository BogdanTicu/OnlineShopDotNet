using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop12.Models
{
    public class OrderProduct
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Order")]
        public int Id_Order { get; set; }

        [ForeignKey("Product")]
        public int Id_Product { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
