using Microsoft.AspNetCore.Mvc;

namespace DoAnLapTrinhWeb.Controllers
{
    public class AdminController : Controller
    {

        // === Quản lý Sản phẩm ===
        public IActionResult Products()
        {
            return RedirectToAction("Index", "Products");
        }

        public IActionResult AddProduct()
        {
            return RedirectToAction("Create", "Products");
        }

        // === Quản lý Danh mục ===
        public IActionResult Categories()
        {
            return RedirectToAction("Index", "Categories");
        }

        public IActionResult AddCategory()
        {
            return RedirectToAction("Create", "Categories");
        }

        // === Quản lý Đơn hàng ===
        public IActionResult Orders()
        {
            return RedirectToAction("Index", "Orders");
        }

        public IActionResult OrderDetails(int id)
        {
            return RedirectToAction("Details", "Orders", new { id });
        }

        // === Quản lý Báo cáo doanh thu ===
        public IActionResult Reports()
        {
            return RedirectToAction("RevenueReport", "Reports");
        }

        // === Quản lý Người dùng ===
        public IActionResult Users()
        {
            return RedirectToAction("Index", "Users");
        }

        public IActionResult AddUser()
        {
            return RedirectToAction("Create", "Users");
        }

        // === Quản lý Liên hệ khách hàng ===
        public IActionResult Contacts()
        {
            return RedirectToAction("Index", "Contacts");
        }

        public IActionResult ContactDetails(int id)
        {
            return RedirectToAction("Details", "Contacts", new { id });
        }

    }
}
