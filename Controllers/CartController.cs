using Microsoft.AspNetCore.Mvc;

namespace Đồ_Án_Lập_Trình_Web_Bán_Thức_Ăn_Nhanh.Controllers
{
    public class CartController : Controller
    {
        // Hiển thị trang giỏ hàng
        public IActionResult Index()
        {
            return View();
        }

        // Xóa sản phẩm khỏi giỏ (demo)
        public IActionResult Remove(int id)
        {
            // TODO: Xóa sản phẩm theo id trong session/database
            TempData["Message"] = "Đã xóa sản phẩm khỏi giỏ!";
            return RedirectToAction("Index");
        }

        // Điều hướng sang trang thông tin khách hàng
        public IActionResult Checkout()
        {
            return RedirectToAction("Index", "Customer");
        }
    }
}
