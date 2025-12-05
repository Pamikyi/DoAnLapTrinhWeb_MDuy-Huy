using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnLapTrinhWebBanThucAnNhanh.Models;
using DoAnLapTrinhWebBanThucAnNhanh.Models.ViewModels;

namespace DoAnLapTrinhWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly FastFoodDbContext _context;

        public AdminController(FastFoodDbContext context)
        {
            _context = context;
        }

        [Route("Admin")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            try
            {
                int productCount = _context.Products.Count();
                int orderCount = _context.CustomerOrders.Count();
                int userCount = _context.UserHLs.Count();

                decimal totalRevenue = _context.CustomerOrders
                    .Sum(o => (decimal?)o.TotalAmount) ?? 0;

                var raw = _context.CustomerOrders
                    .AsNoTracking()
                    .GroupBy(o => new { Year = o.OrderDate.Year, Month = o.OrderDate.Month })
                    .Select(g => new
                    {
                        g.Key.Year,
                        g.Key.Month,
                        Total = g.Sum(x => x.TotalAmount)
                    })
                    .OrderBy(x => x.Year)
                    .ThenBy(x => x.Month)
                    .ToList();

                var vm = new DashboardViewModel
                {
                    TotalProducts = productCount,
                    TotalOrders = orderCount,
                    TotalUsers = userCount,
                    TotalRevenue = totalRevenue,
                    Labels = raw.Select(x => $"{x.Month:00}/{x.Year}").ToList(),
                    Data = raw.Select(x => x.Total).ToList()
                };

                return PartialView("_Dashboard", vm);
            }
            catch (Exception ex)
            {
                return PartialView("_DashboardError", ex.Message);
            }
        }

        public IActionResult ProductList()
        {
            var data = _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .ToList();

            return PartialView("_Products", data);
        }

        public IActionResult CategoryList()
        {
            var data = _context.Categories.AsNoTracking().ToList();
            return PartialView("_Categories", data);
        }

        public IActionResult OrderList()
        {
            var data = _context.CustomerOrders
                .AsNoTracking()
                .Include(o => o.User)
                .ToList();

            return PartialView("_Orders", data);
        }

        public IActionResult UserList()
        {
            var data = _context.UserHLs
                .AsNoTracking()
                .Include(u => u.Role)
                .ToList();

            return PartialView("_Users", data);
        }

        public IActionResult ContactList()
        {
            var data = _context.Contacts
                .AsNoTracking()
                .ToList();

            return PartialView("_Contacts", data);
        }
    }
}
