using System.ComponentModel.DataAnnotations;

namespace DoAnLapTrinhWebBanThucAnNhanh.Models
{
    public class ProductImage
    {
        [Key]                            // 🔥 Thêm dòng này
        public int ImageID { get; set; }

        public int ProductID { get; set; }
        public string ImageUrl { get; set; }

        public Product Product { get; set; }
    }
}
