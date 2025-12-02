using Microsoft.AspNetCore.Mvc;
using DoAnLapTrinhWebBanThucAnNhanh.Models;

public class ContactController : Controller
{
    private readonly FastFoodDbContext _context;

    public ContactController(FastFoodDbContext context)
    {
        _context = context;
    }

    // GET: Contact
    public IActionResult Index()
    {
        return View();
    }

    // POST: Contact
    [HttpPost]
    public IActionResult Index(Contact model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _context.Contacts.Add(model);
        _context.SaveChanges();

        TempData["Success"] = "Gửi liên hệ thành công! Chúng tôi sẽ phản hồi trong thời gian sớm nhất.";

        return RedirectToAction("Index");
    }
}
