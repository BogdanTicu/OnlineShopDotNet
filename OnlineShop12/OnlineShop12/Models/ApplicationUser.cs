using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineShop12.Models
{
    public class ApplicationUser : IdentityUser
    {
        // un user poate posta mai multe articole
        public virtual ICollection<Product>? Products { get; set; }
        // un user poate posta mai multe reviews
        public virtual ICollection<Review>? Reviews { get; set; }

        // un user poate da mai multe ratinguri (TODO)
        public virtual ICollection<Rating>? Ratings { get; set; }

        public string? Phone { get; set; }
        //rolurile unui user
        [NotMapped]
        public IEnumerable<SelectListItem>? AllRoles { get; set; }
    }
}
