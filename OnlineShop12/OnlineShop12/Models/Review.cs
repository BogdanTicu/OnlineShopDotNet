using System.ComponentModel.DataAnnotations;

namespace OnlineShop12.Models
{
    public class Review
    {
        [Key]
        public int Id_Review { get; set; }

        public int Id_User { get; set; }
        public int? Id_Product { get; set; }
        public virtual Product? Product { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
}
