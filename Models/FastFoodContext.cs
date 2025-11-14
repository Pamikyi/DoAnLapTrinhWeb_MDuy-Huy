using Đồ_Án_Lập_Trình_Web_Bán_Thức_Ăn_Nhanh.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnLapTrinhWebBanThucAnNhanh.Models
{
    public class FastFoodContext : DbContext
    {
        public FastFoodContext(DbContextOptions<FastFoodContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserHL> UserHLs { get; set; }
        public DbSet<CustomerOrder> CustomerOrders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Report> Reports { get; set; }
    }
}
