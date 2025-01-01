using System.ComponentModel.DataAnnotations;

namespace OnlineShop12.Models
{

    public class Order
    {
        [Key]
        public int Id_Order { get; set; }
        public string Status { get; set; } = "In cos"; 
        public int Total_Amount { get; set; }

        public DateTime OrderDate { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }

        public string? UserId { get; set; } // Reference to the user
        public virtual ApplicationUser? User { get; set; }

        public virtual Payment? Payment { get; set; }
    }
}
