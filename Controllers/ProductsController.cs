using DoAnLapTrinhWebBanThucAnNhanh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ProductsController : Controller
{
    private readonly FastFoodDbContext _db;

    public ProductsController(FastFoodDbContext db)
    {
        _db = db;
    }

    public IActionResult Details(int id)
    {
        // Lấy sản phẩm + category
        var product = _db.Products
            .Include(p => p.Category)
            .FirstOrDefault(p => p.ProductID == id);

        if (product == null)
            return NotFound();

        // Lấy danh sách hình ảnh
        var images = _db.ProductImages
            .Where(i => i.ProductID == id)
            .ToList();

        // Tạo ViewModel
        var vm = new ProductDetailVM
        {
            Product = product,
            Images = images,
            TatCaSanPham = _db.Products.Take(8).ToList()
        };

        return View(vm);
    }
}
