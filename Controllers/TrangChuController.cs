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

        /// <summary>
        /// Trang chủ: Bán chạy, Món mới, Món tuổi thơ, Danh mục tất cả món
        /// </summary>
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
                .Take(12) // sau này có thể phân trang
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

        // Trang hiển thị tất cả sản phẩm (nếu cần riêng)
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