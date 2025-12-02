namespace DoAnLapTrinhWebBanThucAnNhanh.Models.ViewModels
{
    public class AdminProductVM
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = "";
        public string CategoryName { get; set; } = "";
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}