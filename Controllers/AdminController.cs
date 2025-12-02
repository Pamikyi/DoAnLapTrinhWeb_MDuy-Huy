using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnLapTrinhWebBanThucAnNhanh.Models;
using DoAnLapTrinhWebBanThucAnNhanh.Models.ViewModels;
using System.Linq;

namespace DoAnLapTrinhWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly FastFoodDbContext _context;

        public AdminController(FastFoodDbContext context)
        {
            _context = context;
        }

        // ============================
        // 1️⃣ TRANG ADMIN GỐC
        // ============================
        // /Admin -> load layout lớn
        [Route("Admin")]
        public IActionResult Index()
        {
            return View(); // Views/Admin/Index.cshtml
        }


        // ============================
        // 2️⃣ PARTIAL VIEW CHO SIDEBAR (SPA)
        // ============================

        // DASHBOARD
        public IActionResult Dashboard()
        {
            int productCount = _context.Products.Count();
            int orderCount = _context.CustomerOrders.Count();
            int userCount = _context.UserHLs.Count();

            decimal totalRevenue = _context.CustomerOrders
                .Sum(o => (decimal?)o.TotalAmount) ?? 0;

            var raw = _context.CustomerOrders
                .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Total = g.Sum(x => x.TotalAmount)
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToList();

            var revenueByMonth = raw.Select(x => new
            {
                MonthName = $"{x.Month:00}/{x.Year}",
                Total = x.Total
            }).ToList();

            var vm = new DashboardViewModel
            {
                TotalProducts = productCount,
                TotalOrders = orderCount,
                TotalUsers = userCount,
                TotalRevenue = totalRevenue,
                Labels = revenueByMonth.Select(x => x.MonthName).ToList(),
                Data = revenueByMonth.Select(x => x.Total).ToList()
            };

            return PartialView("_Dashboard", vm);
        }


        // PRODUCT LIST
        public IActionResult ProductList()
        {
            var data = _context.Products
                .Include(p => p.Category)
                .ToList();

            return PartialView("_Products", data);
        }


        // CATEGORY LIST
        public IActionResult CategoryList()
        {
            var data = _context.Categories.ToList();
            return PartialView("_Categories", data);
        }


        // ORDER LIST
        public IActionResult OrderList()
        {
            var data = _context.CustomerOrders
                .Include(o => o.User)
                .ToList();

            return PartialView("_Orders", data);
        }


        // USER LIST
        public IActionResult UserList()
        {
            var data = _context.UserHLs.Include(u => u.Role).ToList();
            return PartialView("_Users", data);
        }


        // CONTACT LIST
        public IActionResult ContactList()
        {
            var data = _context.Contacts.ToList();
            return PartialView("_Contacts", data);
        }


        // ============================
        // 3️⃣ API JSON CHO JS (nếu cần)
        // ============================
        public IActionResult Products()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .Select(p => new AdminProductVM
                {
                    ProductID = p.ProductID,
                    ProductName = p.ProductName,
                    CategoryName = p.Category.CategoryName,
                    Price = p.Price,
                })
                .ToList();

            return Json(products);
        }
    }
}
