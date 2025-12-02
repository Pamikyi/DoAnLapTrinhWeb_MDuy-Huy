using System.Collections.Generic;

namespace DoAnLapTrinhWebBanThucAnNhanh.Models.ViewModels
{
    public class CheckoutViewModel
    {
        // Thông tin giao hàng
        public string ReceiverName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string ShippingAddress { get; set; } = "";
        public string? Request { get; set; }

        // Giỏ hàng
        public List<CartItem> CartItems { get; set; } = new();
        public decimal TotalAmount { get; set; }
    }
}
