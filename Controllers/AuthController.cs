using DoAnLapTrinhWebBanThucAnNhanh.Models;
using DoAnLapTrinhWebBanThucAnNhanh.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DoAnLapTrinhWebBanThucAnNhanh.Controllers
{
    public class AuthController : Controller
    {
        private readonly FastFoodDbContext _context;

        public AuthController(FastFoodDbContext context)
        {
            _context = context;
        }

        // GET: /Auth/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                TempData["Error"] = "Vui lòng nhập đầy đủ thông tin.";
                return View();
            }

            // Hash mật khẩu nhập vào để so sánh
            string passwordHash = HashPassword(password);

            // Kiểm tra user trong database
            var user = await _context.UserHLs
                .FirstOrDefaultAsync(x => x.Username == username && x.Password == passwordHash);

            if (user == null)
            {
                TempData["Error"] = "Tên đăng nhập hoặc mật khẩu không đúng.";
                return View();
            }

            // Lưu session
            HttpContext.Session.SetString("UserName", user.Username);
            HttpContext.Session.SetString("Role", user.RoleID.ToString());
            HttpContext.Session.SetInt32("UserID", user.UserID);

            // Kiểm tra role và điều hướng
            if (user.RoleID == 1)
            {
                // Admin
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                // Người dùng bình thường
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: /Auth/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Kiểm tra trùng tên đăng nhập
            bool usernameExists = await _context.UserHLs.AnyAsync(x => x.Username == model.Username);
            if (usernameExists)
            {
                ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại");
                return View(model);
            }

            // Kiểm tra email tồn tại
            bool emailExists = await _context.UserHLs.AnyAsync(x => x.Email == model.Email);
            if (emailExists)
            {
                ModelState.AddModelError("Email", "Email đã được sử dụng");
                return View(model);
            }

            // Tạo UserHL
            var user = new UserHL
            {
                Username = model.Username,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Password = HashPassword(model.Password), // Hash mật khẩu
                RoleID = 2 // mặc định user bình thường
            };

            _context.UserHLs.Add(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đăng ký tài khoản thành công!";
            return RedirectToAction("Login", "Auth");
        }

        // Hàm HASH mật khẩu
        private string HashPassword(string password)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToHexString(bytes);
            }
        }

        public IActionResult Logout()
        {
            // Xóa toàn bộ session đang lưu
            HttpContext.Session.Clear();

            // Quay về trang đăng nhập hoặc trang chủ
            return RedirectToAction("Login", "Auth");
        }
    }
}
