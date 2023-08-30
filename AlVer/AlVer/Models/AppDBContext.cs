using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AlVer.Models
{
    public class AppDBContext : IdentityDbContext<AppUser,IdentityRole,string>
    {
        public DbSet<Newsletter> newsletters { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ToDoTask> ToDoTasks { get; set; }
        public AppDBContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Basket>(entity =>
            {
                entity.HasOne(b => b.User).WithMany(p => p.Baskets)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Basket_User");

                entity.HasOne(b => b.Product).WithMany(p => p.Baskets)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Basket_Product");
            });

            modelBuilder.Entity<Favorite>(entity =>
            {
                entity.HasOne(b => b.User).WithMany(p => p.Favorites)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Favorite_User");

                entity.HasOne(b => b.Product).WithMany(p => p.Favorites)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Favorite_Product");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property<decimal>(x=> x.Price).HasPrecision(10,2);

                entity.HasOne(p => p.Category).WithMany(p => p.Products)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Product_Category");
            });

            modelBuilder.Entity<Bill> (entity =>
            {
                entity.HasOne(b => b.User).WithMany(u => u.Bills)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Bill_User");

                entity.Property<decimal>(x => x.Price).HasPrecision(10,2);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasOne(o => o.Bill).WithMany(b => b.Orders)
                .HasForeignKey(o => o.BillId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Order_Bill");

                entity.HasOne(o => o.Product).WithMany(p => p.Orders)
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Order_Product");

                entity.HasOne(o => o.User).WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Order_User");

                entity.Property<decimal>(x=> x.Price).HasPrecision(10,2);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
