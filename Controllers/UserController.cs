using Microsoft.AspNetCore.Mvc;
using DoAnLapTrinhWebBanThucAnNhanh.Models;

public class UserController : Controller
{
    private readonly FastFoodDbContext _context;

    public UserController(FastFoodDbContext context)
    {
        _context = context;
    }

    public IActionResult Profile()
    {
        int? userId = HttpContext.Session.GetInt32("UserID");
        if (userId == null)
            return RedirectToAction("Login", "Auth");

        var user = _context.UserHLs.FirstOrDefault(u => u.UserID == userId);
        return View(user);
    }

    [HttpPost]
    public IActionResult Profile(UserHL model)
    {
        var user = _context.UserHLs.FirstOrDefault(u => u.UserID == model.UserID);

        if (user == null)
            return NotFound();

        // Cập nhật thông tin cho user
        user.Email = model.Email;
        user.PhoneNumber = model.PhoneNumber;
        user.Username = model.Username;

        _context.SaveChanges();

        TempData["Success"] = "Cập nhật thông tin thành công!";
        return RedirectToAction("Profile");
    }
}
