using System.Collections.Generic;

namespace DoAnLapTrinhWebBanThucAnNhanh.Models
{
    public class HomeIndexViewModel
    {
        public List<Product> BanChay { get; set; } = new();
        public List<Product> MonMoi { get; set; } = new();
        public List<Product> MonTuoiTho { get; set; } = new();

        public List<Product> TatCaSanPham { get; set; } = new();

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
