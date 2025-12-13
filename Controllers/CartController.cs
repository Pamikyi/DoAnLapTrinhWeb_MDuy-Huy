using DoAnLapTrinhWebBanThucAnNhanh.Models;
using DoAnLapTrinhWebBanThucAnNhanh.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

public class CartController : Controller
{
    private readonly FastFoodDbContext _context;

    public CartController(FastFoodDbContext context)
    {
        _context = context;
    }
    public IActionResult Add(int id)
    {
        // Lấy cart từ Session hoặc tạo mới
        var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart")
                   ?? new List<CartItem>();

        // Lấy sản phẩm theo id
        var product = _context.Products.FirstOrDefault(p => p.ProductID == id);
        if (product == null)
        {
            return NotFound();
        }

        // Kiểm tra đã có trong giỏ chưa
        var existingItem = cart.FirstOrDefault(c => c.ProductID == id);

        if (existingItem == null)
        {
            cart.Add(new CartItem
            {
                ProductID = product.ProductID,
                Name = product.ProductName,
                Price = product.Price,
                Quantity = 1,
                Image = product.ImageURL ?? ""
            });
        }
        else
        {
            existingItem.Quantity++;
        }

        // Lưu session
        HttpContext.Session.SetObject("Cart", cart);

        // 👉 Chuyển thẳng đến trang giỏ hàng
        return RedirectToAction("Index", "Cart");
    }



    public IActionResult Index()
    {
        var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart")
                   ?? new List<CartItem>();

        return View(cart);
    }
    public IActionResult UpdateQuantity(int id, int quantity)
    {
        var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new();

        var item = cart.FirstOrDefault(x => x.ProductID == id);
        if (item != null)
        {
            item.Quantity = quantity;
        }

        HttpContext.Session.SetObject("Cart", cart);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Checkout(string ReceiverName, string Phone, string ShippingAddress, string? Request)
    {
        var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new();

        if (!cart.Any())
            return RedirectToAction("Index");

        // Lấy user đang đăng nhập (UserID đã lưu trong Session)
        int? userId = HttpContext.Session.GetInt32("UserID");

        if (userId == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        decimal total = cart.Sum(x => x.Price * x.Quantity);

        // 1) Tạo đơn hàng CustomerOrder
        var order = new CustomerOrder
        {
            UserID = userId.Value,
            ReceiverName = ReceiverName,
            Phone = Phone,
            ShippingAddress = ShippingAddress,
            Request = Request,
            OrderDate = DateTime.Now,
            TotalAmount = total,
            Status = "Pending",
            PaymentMethod = "COD"
        };

        _context.CustomerOrders.Add(order);
        _context.SaveChanges();  // Lưu để lấy CustomerOrdersID

        // 2) Tạo từng dòng OrderDetail
        foreach (var item in cart)
        {
            var detail = new OrderDetail
            {
                CustomerOrderID = order.CustomerOrdersID,
                ProductID = item.ProductID,
                Quantity = item.Quantity,
                Price = item.Price
            };

            _context.OrderDetails.Add(detail);
        }

        _context.SaveChanges();

        // 3) Xoá giỏ hàng
        HttpContext.Session.Remove("Cart");

        // 4) Chuyển sang trang thành công
        return RedirectToAction("Success", new { id = order.CustomerOrdersID });
    }


    public IActionResult Success(int id)
    {
        var order = _context.CustomerOrders.FirstOrDefault(o => o.CustomerOrdersID == id);
        return View(order);
    }
    // ------- HIỂN THỊ TRANG CHECKOUT -------
    [HttpGet]
    [HttpGet]
    public IActionResult Checkout()
    {
        var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart")
                   ?? new List<CartItem>();

        if (!cart.Any())
            return RedirectToAction("Index");

        // ---- Lấy user đang đăng nhập ----
        int? userId = HttpContext.Session.GetInt32("UserID");
        UserHL? user = null;

        if (userId != null)
        {
            user = _context.UserHLs.FirstOrDefault(u => u.UserID == userId);
        }

        // ---- Tạo ViewModel & Auto fill dữ liệu ----
        var vm = new CheckoutViewModel
        {
            CartItems = cart,
            TotalAmount = cart.Sum(x => x.Price * x.Quantity),

            ReceiverName = user?.Username ?? "",
            Phone = user?.PhoneNumber ?? "",
            ShippingAddress = user?.Address ?? ""   // nếu bạn có Address trong UserHL
        };

        return View(vm);
    }
    // Cart remove
    public IActionResult Remove(int id)
    {
        var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart")
                   ?? new List<CartItem>();

        var item = cart.FirstOrDefault(x => x.ProductID == id);
        if (item != null)
        {
            cart.Remove(item);
        }

        HttpContext.Session.SetObject("Cart", cart);

        return RedirectToAction("Index");
    }

}