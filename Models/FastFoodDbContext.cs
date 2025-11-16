using Microsoft.EntityFrameworkCore;

namespace DoAnLapTrinhWebBanThucAnNhanh.Models
{
    public class FastFoodDbContext : DbContext
    {
        public FastFoodDbContext(DbContextOptions<FastFoodDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserHL> UserHLs { get; set; }
        public DbSet<CustomerOrder> CustomerOrders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Report> Reports { get; set; }

        // ⚠️ THÊM BẢNG IMAGE VÀO ĐÂY
        public DbSet<ProductImage> ProductImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique category name
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.CategoryName)
                .IsUnique();
        }
    }
}
