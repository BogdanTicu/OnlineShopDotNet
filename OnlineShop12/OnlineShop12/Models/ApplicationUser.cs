using Microsoft.AspNetCore.Identity;

namespace OnlineShop12.Models
{
    public class ApplicationUser : IdentityUser
    {
        // un user poate posta mai multe articole
        public virtual ICollection<Product>? Products { get; set; }
        // un user poate posta mai multe reviews
        public virtual ICollection<Review>? Reviews { get; set; }

        // un user poate da mai multe ratinguri TODO

        public string? Phone { get; set; }
    }
}
