using System.ComponentModel.DataAnnotations;

namespace OnlineShop12.Models
{
    public class Category
    {
        [Key]
        public int Id_Category { get; set; }
        [Required(ErrorMessage = "Numele este obligatoriu")]
        public string? Category_Name { get; set; }
        [Required(ErrorMessage = "Descrierea este obligatorie")]
        public string? Description { get; set; }

        public virtual ApplicationUser? User { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
    }
}
