using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnLapTrinhWebBanThucAnNhanh.Models;
using DoAnLapTrinhWebBanThucAnNhanh.Models.ViewModels;

namespace DoAnLapTrinhWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly FastFoodDbContext _context;
        private readonly IWebHostEnvironment _env;

        // ✅ SINGLE VALID CONSTRUCTOR
        public AdminController(FastFoodDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ---------------------------------------------------
        // 1) LOAD TRANG ADMIN
        // ---------------------------------------------------
        [Route("Admin")]
        public IActionResult Index()
        {
            return View(); // Views/Admin/Index.cshtml
        }

        // ---------------------------------------------------
        // 2) DASHBOARD — PartialView
        // ---------------------------------------------------
        [HttpGet]
        public IActionResult Dashboard()
        {
            var productCount = _context.Products.Count();
            var orderCount = _context.CustomerOrders.Count();
            var userCount = _context.UserHLs.Count();

            decimal total = _context.CustomerOrders
                .Sum(o => (decimal?)o.TotalAmount) ?? 0;

            var revenueRaw = _context.CustomerOrders
                .AsNoTracking()
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

            var vm = new DashboardViewModel
            {
                TotalProducts = productCount,
                TotalOrders = orderCount,
                TotalUsers = userCount,
                TotalRevenue = total,
                Labels = revenueRaw.Select(x => $"{x.Month:00}/{x.Year}").ToList(),
                Data = revenueRaw.Select(x => x.Total).ToList()
            };

            return PartialView("_Dashboard", vm);
        }

        // ---------------------------------------------------
        // 3) PRODUCT LIST — PartialView
        // ---------------------------------------------------
        [HttpGet]
        public IActionResult ProductList()
        {
            var data = _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .ToList();

            return PartialView("_Products", data);
        }

<<<<<<< HEAD
        // ---------------------------------------------------
        // 4) CATEGORY LIST — PartialView
        // ---------------------------------------------------
        [HttpGet]
        public IActionResult CategoryList()
        {
            var data = _context.Categories.AsNoTracking().ToList();
            return PartialView("_Categories", data);
        }
=======
          public IActionResult CategoryList()
  {
      var data = _context.Categories
          .AsNoTracking()
          .Include(c => c.Products)   // Quan trọng để đếm món
          .ToList();

      return PartialView("_Categories", data);
  }
>>>>>>> 1b9a1cc19799d4c47eeafba026071668cb0f6d52

        // ---------------------------------------------------
        // 5) ORDERS LIST — PartialView
        // ---------------------------------------------------
        [HttpGet]
        public IActionResult OrderList()
        {
            var data = _context.CustomerOrders.AsNoTracking().ToList();
            return PartialView("_Orders", data);
        }

        // ---------------------------------------------------
        // 6) USER LIST — PartialView
        // ---------------------------------------------------
        [HttpGet]
        public IActionResult UserList()
        {
            var data = _context.UserHLs
                .Include(r => r.Role)
                .AsNoTracking()
                .ToList();

            return PartialView("_Users", data);
        }

        // ---------------------------------------------------
        // 7) CONTACT LIST — PartialView
        // ---------------------------------------------------
        [HttpGet]
        public IActionResult ContactList()
        {
            var data = _context.Contacts.AsNoTracking().ToList();
            return PartialView("_Contacts", data);
        }

        // ---------------------------------------------------
        // 8) CREATE PRODUCT — GET
        // ---------------------------------------------------
        [HttpGet]
        public IActionResult CreateProduct()
        {
            ViewBag.Categories = _context.Categories.ToList();

            return PartialView("_CreateProduct", new Product());
        }


        // ---------------------------------------------------
        // 9) CREATE PRODUCT — POST
        // ---------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProduct(Product model, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                return PartialView("_CreateProduct", model);
            }

            // Upload ảnh (nếu có)
            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot/uploads/products");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                imageFile.CopyTo(stream);

                model.ImageURL = "/uploads/products/" + fileName;
            }

            // 👉 GHI DB
            _context.Products.Add(model);
            _context.SaveChanges();

            // 👉 TRẢ LẠI LIST (SPA)
            return ProductList();
        }


        // ---------------------------------------------------
        // 10) EDIT PRODUCT — GET
        // ---------------------------------------------------
        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            var p = _context.Products.AsNoTracking().FirstOrDefault(x => x.ProductID == id);
            if (p == null) return NotFound();

            LoadCategories();
            return PartialView("_EditProduct", p);
        }

        // ---------------------------------------------------
        // 11) EDIT PRODUCT — POST
        // ---------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProduct(Product model, IFormFile? imageFile)
        {
            var p = _context.Products.FirstOrDefault(x => x.ProductID == model.ProductID);
            if (p == null) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                return PartialView("_EditProduct", model);
            }

            // update field
            p.ProductName = model.ProductName;
            p.Price = model.Price;
            p.CategoryID = model.CategoryID;
            p.Descriptions = model.Descriptions;
            p.DetailDescription = model.DetailDescription;
            p.Ingredients = model.Ingredients;

            // bool (đã chuẩn hóa DB)
            p.IsBestSeller = model.IsBestSeller;
            p.IsNew = model.IsNew;
            p.IsChildhoodDish = model.IsChildhoodDish;
            p.IsAvailable = model.IsAvailable;

            // ✅ CHỈ update ảnh NẾU user chọn ảnh mới
            if (imageFile != null && imageFile.Length > 0)
            {
                var folder = Path.Combine(_env.WebRootPath, "uploads", "products");
                Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                var path = Path.Combine(folder, fileName);

                using var stream = new FileStream(path, FileMode.Create);
                imageFile.CopyTo(stream);

                p.ImageURL = "/uploads/products/" + fileName;
            }
            // ❌ KHÔNG ELSE → giữ ảnh cũ

            _context.SaveChanges();
            return ProductList();
        }


        // ---------------------------------------------------
        // 12) DELETE PRODUCT — POST
        // ---------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProduct([FromForm] int id)
        {
            var p = _context.Products.Find(id);
            if (p == null) return NotFound();

            _context.Products.Remove(p);
            _context.SaveChanges();

            return ProductList();
        }

        // ===================================================
        // HELPERS
        // ===================================================
        private void LoadCategories()
        {
            ViewBag.Categories = _context.Categories.AsNoTracking().ToList();
        }

        private string? SaveProductImage(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0) return null;

            var folder = Path.Combine(_env.WebRootPath, "uploads", "products");
            Directory.CreateDirectory(folder);

            var ext = Path.GetExtension(imageFile.FileName);
            var fileName = $"{Guid.NewGuid():N}{ext}";
            var filePath = Path.Combine(folder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            imageFile.CopyTo(stream);

            return "/uploads/products/" + fileName;
        }
    }
}

