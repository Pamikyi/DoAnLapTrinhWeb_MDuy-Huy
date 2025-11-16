namespace DoAnLapTrinhWebBanThucAnNhanh.Models
{
    public class ProductDetailVM
    {
        public Product Product { get; set; }
        public List<ProductImage> Images { get; set; }
        public List<Product> TatCaSanPham { get; set; }
    }
}
