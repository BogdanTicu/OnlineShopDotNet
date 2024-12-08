using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop12.Models
{
    public class OrderProduct
    {
        
        public int? Id_Order {  get; set; }

        public int? Id_Product { get; set; }

        public virtual Order? Order { get; set; }

        public virtual Product? Product { get; set; }

        //public int Quantity { get; set; }
    }
}
