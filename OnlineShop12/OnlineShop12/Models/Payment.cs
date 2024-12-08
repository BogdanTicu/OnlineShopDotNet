using System.ComponentModel.DataAnnotations;

namespace OnlineShop12.Models
{
    public class Payment
    {
        [Key]
        public int Id_Payment { get; set; }

        public int Id_Order {  get; set; }
        public virtual Order Order { get; set; }

        public string Method { get; set; }

        public string Address { get; set; }

        public DateTime Order_Date { get; set; }
    }
}
