using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnLapTrinhWebBanThucAnNhanh.Models;
using DoAnLapTrinhWebBanThucAnNhanh.ViewModels;

namespace DoAnLapTrinhWebBanThucAnNhanh.Controllers
{
    public class ProfileController : Controller
    {
        private readonly FastFoodDbContext _context;

        public ProfileController(FastFoodDbContext context)
        {
            _context = context;
        }

        // ===============================
        // GET: /Profile
        // ===============================
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var user = _context.UserHLs
                .Include(u => u.Role)
                .FirstOrDefault(u => u.UserID == userId);

            return View("~/Views/User/Profile.cshtml", user);
        }
        // ===============================
        // POST: /Profile/Update
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(ProfileUpdateVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu không hợp lệ";
                return RedirectToAction("Index");
            }

            var user = _context.UserHLs
                .FirstOrDefault(u => u.UserID == model.UserID);

            if (user == null)
                return NotFound();

            // ✅ CHỈ UPDATE NHỮNG FIELD ĐƯỢC PHÉP
            user.Username = model.Username;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;

            // ❌ TUYỆT ĐỐI KHÔNG ĐỘNG:
            // user.Password
            // user.RoleID

            _context.SaveChanges();

            TempData["Success"] = "Cập nhật thông tin thành công";
            return RedirectToAction("Index");
        }
    }
}
