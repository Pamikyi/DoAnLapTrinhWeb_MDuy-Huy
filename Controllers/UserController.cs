using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnLapTrinhWebBanThucAnNhanh.Models;
using DoAnLapTrinhWebBanThucAnNhanh.ViewModels;

[Route("User")]
public class UserController : Controller
{
    private readonly FastFoodDbContext _context;

    public UserController(FastFoodDbContext context)
    {
        _context = context;
    }

    // ===============================
    // GET: /User/Profile
    // ===============================
    [HttpGet("Profile")]
    public IActionResult Profile()
    {
        int? userId = HttpContext.Session.GetInt32("UserID");
        if (userId == null)
            return RedirectToAction("Login", "Auth");

        var user = _context.UserHLs
            .Include(u => u.Role)
            .FirstOrDefault(u => u.UserID == userId);

        if (user == null)
            return RedirectToAction("Login", "Auth");

        var orders = _context.CustomerOrders
            .Where(o => o.UserID == userId)
            .OrderByDescending(o => o.OrderDate)
            .ToList();

        var vm = new UserProfileVM
        {
            User = user,
            Orders = orders
        };

        return View(vm);
    }

    // ===============================
    // POST: /User/Profile/Update
    // ===============================
    [HttpPost("Profile/Update")]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateProfile(UserHL model)
    {
        var user = _context.UserHLs.FirstOrDefault(u => u.UserID == model.UserID);
        if (user == null)
            return NotFound();

        user.Username = model.Username;
        user.Email = model.Email;
        user.PhoneNumber = model.PhoneNumber;
        user.Address = model.Address;

        _context.SaveChanges();

        TempData["Success"] = "Cập nhật thông tin thành công!";
        return RedirectToAction("Profile");
    }
}
