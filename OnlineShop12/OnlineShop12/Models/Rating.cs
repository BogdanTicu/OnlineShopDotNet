using System.ComponentModel.DataAnnotations;

namespace OnlineShop12.Models
{
    public class Rating
    {
        [Key]
        public int Id_Rating { get; set; }

        public string? Id_User { get; set; } = string.Empty;

        public int? Id_Product { get; set; }
        public virtual Product? Product { get; set; }
        public virtual ApplicationUser? User { get; set; }

        public DateTime Date {get; set; }

        public int Value {  get; set; }
    }
}
