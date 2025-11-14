using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Đồ_Án_Lập_Trình_Web_Bán_Thức_Ăn_Nhanh.Models
{
    [Table("Products")] // 🔹 Mapping đúng với tên bảng trong SQL
    public class Product
    {
        [Key]
        [StringLength(5)]
        [Display(Name = "Mã sản phẩm")]
        public string ProductID { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(50, ErrorMessage = "Tên sản phẩm tối đa 50 ký tự")]
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; } = string.Empty;

        [Display(Name = "Mô tả sản phẩm")]
        public string? Descriptions { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm không được để trống")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0")]
        [Display(Name = "Giá bán (VNĐ)")]
        public decimal Price { get; set; }

        [StringLength(255)]
        [Display(Name = "Đường dẫn hình ảnh")]
        public string? ImageURL { get; set; }

        [Display(Name = "Sản phẩm bán chạy")]
        public bool IsBestSeller { get; set; }

        [Display(Name = "Sản phẩm mới")]
        public bool IsNew { get; set; }

        [Display(Name = "Món tuổi thơ")]
        public bool IsChildhoodDish { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // 🔸 Khóa ngoại liên kết với bảng Category
        [Required]
        [Display(Name = "Danh mục")]
        public int CategoryID { get; set; }

        [ForeignKey(nameof(CategoryID))]
        public virtual Category Category { get; set; }
    }
}
