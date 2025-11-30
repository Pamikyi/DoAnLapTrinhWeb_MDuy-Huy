using Microsoft.AspNetCore.Mvc;
using DoAnLapTrinhWebBanThucAnNhanh.Models;
using Microsoft.EntityFrameworkCore;

public class ChiTietSanPhamController : Controller
{
    private readonly FastFoodDbContext _context;

    public ChiTietSanPhamController(FastFoodDbContext context)
    {
        _context = context;
    }

    public IActionResult Index(int id)
    {
        // Lấy sản phẩm theo ID + cả Category
        var product = _context.Products
            .Include(p => p.Category)
            .FirstOrDefault(p => p.ProductID == id);

        if (product == null)
        {
            return NotFound();
        }

        // Lấy nhiều ảnh của sản phẩm
        var images = _context.ProductImages
            .Where(i => i.ProductID == id)
            .ToList();

        // Tạo ViewModel
        var vm = new ProductDetailVM
        {
            Product = product,
            Images = images
        };

        return View(vm);
    }
}
