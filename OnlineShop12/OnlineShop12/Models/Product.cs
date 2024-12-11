using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineShop12.Models
{
    public class Product
    {
        [Key]
        public int Id_Product { get; set; }
        [Required(ErrorMessage ="Categoria este obligatorie")]
        public int? Id_Category {  get; set; }
         public virtual Category? Category { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem>? Categ { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public int Stock { get; set; }

        public int? Score { get; set; }

        public virtual ApplicationUser? User { get; set; }
        public virtual ICollection<Rating>? Ratings { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
        public virtual ICollection<OrderProduct>? OrderProducts { get; set; }
    }
}
