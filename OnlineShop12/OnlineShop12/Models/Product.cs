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
        [Required(ErrorMessage = "Titlul este obligatoriu")]
        public string Title { get; set; }
        public bool isApproved { get; set; } = false;
        public bool isDeleted { get; set; }
        [Required(ErrorMessage = "Descrierea este obligatorie")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Pretul este obligatoriu")]
        public int? Price { get; set; }
        [Required(ErrorMessage = "Stocul este obligatoriu")]
        public int? Stock { get; set; }
        public double? Score { get; set; }

        public string? ImagePath { get; set; }


        public virtual ApplicationUser? User { get; set; }
        public virtual ICollection<Rating>? Ratings { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
        public virtual ICollection<OrderProduct>? OrderProducts { get; set; }
    }
}
