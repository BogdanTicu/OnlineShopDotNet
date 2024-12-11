using System.ComponentModel.DataAnnotations;

namespace OnlineShop12.Models
{
    public class Category
    {
        [Key]
        public int Id_Category { get; set; }

        public string Category_Name { get; set; }

        public string Description { get; set; }

        public virtual ApplicationUser? User { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
    }
}
