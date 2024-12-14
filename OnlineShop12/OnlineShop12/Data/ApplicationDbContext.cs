using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineShop12.Models;

namespace OnlineShop12.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderProduct> OrderProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // definire primary key compus
            modelBuilder.Entity<OrderProduct>()
            .HasKey(ac => new { ac.Id_Order, ac.Id_Product });

            // definire relatii cu modelele FK
            modelBuilder.Entity<OrderProduct>()
            .HasOne(ac => ac.Order)
            .WithMany(ac => ac.OrderProducts)
            .HasForeignKey(ac => ac.Id_Order)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderProduct>()
            .HasOne(ac => ac.Product)
            .WithMany(ac => ac.OrderProducts)
            .HasForeignKey(ac => ac.Id_Product)
            .OnDelete(DeleteBehavior.Cascade);



            base.OnModelCreating(modelBuilder);
            // definire primary key compus


            modelBuilder.Entity<Review>()
           .HasOne<Product>(a => a.Product)
           .WithMany(c => c.Reviews)
           .HasForeignKey(a => a.Id_Product)
           .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<Product>()
          .HasOne<Category>(a => a.Category)
          .WithMany(c => c.Products)
          .HasForeignKey(a => a.Id_Category);


            modelBuilder.Entity<Rating>()
          .HasOne<Product>(a => a.Product)
          .WithMany(c => c.Ratings)
          .HasForeignKey(a => a.Id_Product)
          .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Payment>()
            .HasOne<Order>(a => a.Order)
            .WithOne(c => c.Payment)
            .HasForeignKey<Payment>(a => a.Id_Order)
            .OnDelete(DeleteBehavior.Cascade); // Controlează propagarea ștergerii
        }





    }

}
