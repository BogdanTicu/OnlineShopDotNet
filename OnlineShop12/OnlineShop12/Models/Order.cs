using System.ComponentModel.DataAnnotations;

namespace OnlineShop12.Models
{
    public class Order
    {
        [Key]
        public int Id_Order { get; set; }

        public string Status { get; set; }

        public int Total_Amount {  get; set; }

        public virtual Payment Payment { get; set; }
        public virtual ICollection<OrderProduct>? OrderProducts { get; set; }

    }
}
