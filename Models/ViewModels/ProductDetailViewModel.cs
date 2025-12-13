namespace DoAnLapTrinhWebBanThucAnNhanh.Models
{
    public class ProductDetailViewModel
    {
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public string? Descriptions { get; set; }   // mô tả ngắn
        public decimal Price { get; set; }
        public string? CategoryName { get; set; }

        public required List<string> Images { get; set; }   // nhiều hình
        public string? DetailDescription { get; set; }   // mô tả dài
    }

}