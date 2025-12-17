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

        // ✅ SINGLE VALID CONSTRUCTOR (GIỮ NGUYÊN)
        public AdminController(FastFoodDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ===================================================
        // 1) ADMIN INDEX
        // ===================================================
        [Route("Admin")]
        public IActionResult Index()
        {
            return View(); // Views/Admin/Index.cshtml
        }

        // ===================================================
        // 2) DASHBOARD — PartialView
        // ===================================================
        [HttpGet]
        public IActionResult Dashboard()
        {
            var productCount = _context.Products.Count();
            var orderCount = _context.CustomerOrders.Count();
            var userCount = _context.UserHLs.Count();

            decimal totalRevenue = _context.CustomerOrders
                .Sum(o => (decimal?)o.TotalAmount) ?? 0;

            var revenueRaw = _context.CustomerOrders
                .AsNoTracking()
                .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
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
                Labels = revenueRaw.Select(x => $"{x.Month:00}/{x.Year}").ToList(),
                Data = revenueRaw.Select(x => x.Total).ToList()
            };

            return PartialView("_Dashboard", vm);
        }

        // ===================================================
        // 3) PRODUCT LIST — PartialView
        // ===================================================
        [HttpGet]
        public IActionResult ProductList()
        {
            var data = _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .ToList();

            return PartialView("_Products", data);
        }

        // ===================================================
        // 4) CATEGORY LIST — PartialView
        // ===================================================
        [HttpGet]
        public IActionResult CategoryList()
        {
            var data = _context.Categories.AsNoTracking().ToList();
            return PartialView("_Categories", data);
        }

        // ===================================================
        // 5) ORDER LIST — PartialView
        // ===================================================
        [HttpGet]
        public IActionResult OrderList()
        {
            var data = _context.CustomerOrders
                .AsNoTracking()
                .ToList();

            return PartialView("_Orders", data);
        }

        // ===================================================
        // 6) USER LIST — PartialView
        // ===================================================
        [HttpGet]
        public IActionResult UserList()
        {
            var data = _context.UserHLs
                .Include(u => u.Role)
                .AsNoTracking()
                .ToList();

            return PartialView("_Users", data);
        }

        // ===================================================
        // 7) CONTACT LIST — PartialView
        // ===================================================
        // ===================================================
        [HttpGet]
        public IActionResult ContactList()
        {
            var data = _context.Contacts
                .AsNoTracking()
                .OrderByDescending(c => c.CreateAt)
                .ToList();

            return PartialView("_Contacts", data);
        }
        // ===================================================
        // CONTACT: DELETE
        // ===================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteContact(int id)
        {
            var c = _context.Contacts.Find(id);
            if (c == null) return NotFound();

            _context.Contacts.Remove(c);
            _context.SaveChanges();

            return ContactList();
        }
        // ===================================================
        // 8) CREATE PRODUCT — GET
        // ===================================================
        [HttpGet]
        public IActionResult CreateProduct()
        {
            LoadCategories();
            return PartialView("AdminModule/_CreateProduct", new Product());
        }

        // ===================================================
        // 9) CREATE PRODUCT — POST
        // ===================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProduct(Product model, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                LoadCategories();
                return PartialView("AdminModule/_CreateProduct", model);
            }

            // NEW: dùng helper SaveProductImage (thay vì lặp code)
            var imageUrl = SaveProductImage(imageFile);
            if (imageUrl != null)
                model.ImageURL = imageUrl;

            _context.Products.Add(model);
            _context.SaveChanges();

            // SPA: trả lại danh sách
            return ProductList();
        }

        // ===================================================
        // 10) EDIT PRODUCT — GET
        // ===================================================
        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            var p = _context.Products
                .AsNoTracking()
                .FirstOrDefault(x => x.ProductID == id);

            if (p == null) return NotFound();

            LoadCategories();
            return PartialView("AdminModule/_EditProduct", p);
        }

        // ===================================================
        // 11) EDIT PRODUCT — POST
        // ===================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProduct(Product model, IFormFile? imageFile)
        {
            var p = _context.Products.FirstOrDefault(x => x.ProductID == model.ProductID);
            if (p == null) return NotFound();

            if (!ModelState.IsValid)
            {
                LoadCategories();
                return PartialView("AdminModule/_EditProduct", model);
            }

            // update field (GIỮ NGUYÊN)
            p.ProductName = model.ProductName;
            p.Price = model.Price;
            p.CategoryID = model.CategoryID;
            p.Descriptions = model.Descriptions;
            p.DetailDescription = model.DetailDescription;
            p.Ingredients = model.Ingredients;

            // bool đã chuẩn hóa DB
            p.IsBestSeller = model.IsBestSeller;
            p.IsNew = model.IsNew;
            p.IsChildhoodDish = model.IsChildhoodDish;
            p.IsAvailable = model.IsAvailable;

            // NEW: chỉ update ảnh nếu có upload mới
            var imageUrl = SaveProductImage(imageFile);
            if (imageUrl != null)
                p.ImageURL = imageUrl;

            _context.SaveChanges();
            return ProductList();
        }

        // ===================================================
        // 12) DELETE PRODUCT — POST
        // ===================================================
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
        // 12) DELETE PRODUCT — POST
        // ===================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory([FromForm] int id)
        {
            var p = _context.Categories.Find(id);
            if (p == null) return NotFound();

            _context.Categories.Remove(p);
            _context.SaveChanges();

            return CategoryList();
        }

        // ===================================================
        // HELPERS (GIỮ + TÁI SỬ DỤNG)
        // ===================================================
        private void LoadCategories()
        {
            ViewBag.Categories = _context.Categories
                .AsNoTracking()
                .ToList();
        }

        // NEW: gom logic upload ảnh (dùng cho Create + Edit)
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

        // ===================================================
        // CREATE CATEGORY — GET
        // ===================================================
        [HttpGet]
        public IActionResult CreateCategory()
        {
            // NEW: chỉ dùng để render form tạo danh mục (SPA)
            // Không load layout, không xử lý DB

            var model = new Category(); // NEW: model rỗng cho form

            return PartialView("AdminModule/_CreateCategory", model);
        }
        // ===================================================
        // CREATE CATEGORY — POST
        // ===================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCategory(Category model)
        {
            if (!ModelState.IsValid)
            {
                // NEW: trả lại form nếu lỗi
                return PartialView("AdminModule/_CreateCategory", model);
            }

            // NEW: ghi DB
            _context.Categories.Add(model);
            _context.SaveChanges();

            // NEW: trả lại danh sách Category (SPA)
            return CategoryList();
        }
        [HttpGet]
        public IActionResult EditUser(int id)
        {
            var user = _context.UserHLs
                .AsNoTracking()
                .FirstOrDefault(u => u.UserID == id);

            if (user == null) return NotFound();

            ViewBag.Roles = _context.Roles.ToList();

            var vm = new EditUserVM
            {
                UserID = user.UserID,
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                RoleID = user.RoleID
            };

            return PartialView("AdminModule/_EditUser", vm);
        }

        // ===================================================
        // EDIT USER — POST
        // ===================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditUser(EditUserVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _context.Roles.ToList();
                return PartialView("AdminModule/_EditUser", model);
            }

            var user = _context.UserHLs.FirstOrDefault(u => u.UserID == model.UserID);
            if (user == null) return NotFound();

            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.RoleID = model.RoleID;

            _context.SaveChanges();

            return UserList();
        }
        // ===================================================
        // ORDER: DETAIL — PartialView
        // ===================================================
        [HttpGet]
        public IActionResult OrderDetail(int id)
        {
            var order = _context.CustomerOrders
                .Include(o => o.OrderDetails)
                    .ThenInclude(d => d.Product)
                .AsNoTracking()
                .FirstOrDefault(o => o.CustomerOrdersID == id);

            if (order == null)
                return NotFound();

            return PartialView("AdminModule/_OrderDetail", order);
        }
        // ===================================================
        // ORDER: DELETE
        // ===================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteOrder(int id)
        {
            var order = _context.CustomerOrders
                .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.CustomerOrdersID == id);

            if (order == null)
                return NotFound();

            // ❗ Xoá chi tiết trước (FK)
            _context.OrderDetails.RemoveRange(order.OrderDetails);

            _context.CustomerOrders.Remove(order);
            _context.SaveChanges();

            return OrderList(); // SPA: render lại danh sách
        }
        // ===================================================
        // USER: DELETE
        // ===================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUser([FromForm] int id)
        {
            var user = _context.UserHLs
                .Include(u => u.Role)
                .FirstOrDefault(u => u.UserID == id);

            if (user == null)
                return NotFound();

            // NEW: chặn xoá Admin (tuỳ bạn có RoleName khác thì đổi lại)
            if (user.Role?.RoleName?.ToLower() == "admin")
                return BadRequest("Không được xoá tài khoản Admin.");

            _context.UserHLs.Remove(user);
            _context.SaveChanges();

            return UserList(); // SPA: render lại danh sách
        }
    }
}
