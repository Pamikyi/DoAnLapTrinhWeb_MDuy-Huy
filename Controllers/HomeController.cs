using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnLapTrinhWebBanThucAnNhanh.Models;
using System.Diagnostics;
using System.Linq;

namespace DoAnLapTrinhWebBanThucAnNhanh.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FastFoodDbContext _context;

        public HomeController(ILogger<HomeController> logger, FastFoodDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var sanPhamBanChay = await _context.Products
                .Where(s => s.IsBestSeller)
                .OrderByDescending(s => s.CreatedAt)
                .Take(8)
                .ToListAsync();

            var sanPhamMoi = await _context.Products
                .Where(s => s.IsNew)
                .OrderByDescending(s => s.CreatedAt)
                .Take(8)
                .ToListAsync();

            var sanPhamTuoiTho = await _context.Products
                .Where(s => s.IsChildhoodDish)
                .OrderByDescending(s => s.CreatedAt)
                .Take(8)
                .ToListAsync();

            var tatCaSanPham = await _context.Products
                .OrderByDescending(s => s.CreatedAt)
                .Take(12)
                .ToListAsync();

            var model = new HomeIndexViewModel
            {
                BanChay = sanPhamBanChay,
                MonMoi = sanPhamMoi,
                MonTuoiTho = sanPhamTuoiTho,
                TatCaSanPham = tatCaSanPham
            };

            return View(model);
        }

        public async Task<IActionResult> Products()
        {
            var sanPhams = await _context.Products
                .Include(p => p.Category)
                .ToListAsync();

            return View(sanPhams);
        }

        public IActionResult About() => View();
        public IActionResult Contact() => View();
        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
