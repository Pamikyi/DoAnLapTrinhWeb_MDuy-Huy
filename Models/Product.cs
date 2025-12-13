using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnLapTrinhWebBanThucAnNhanh.Models
{
    [Table("Products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Mã sản phẩm")]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(50, ErrorMessage = "Tên sản phẩm tối đa 50 ký tự")]
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; } = string.Empty;

        [Display(Name = "Mô tả sản phẩm")]
        [StringLength(1000)]
        public string? Descriptions { get; set; }
        [Display(Name = "Mô tả chi tiết")]
        public string? DetailDescription { get; set; }
        [Display(Name = "Thành phần")]
        public string? Ingredients { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm không được để trống")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0")]
        [Display(Name = "Giá bán (VNĐ)")]
        public decimal Price { get; set; }

        [StringLength(255)]
        [Display(Name = "Đường dẫn hình ảnh")]
        public string? ImageURL { get; set; }

        [Display(Name = "Sản phẩm bán chạy")]
        public bool IsBestSeller { get; set; } = false;

        [Display(Name = "Sản phẩm mới")]
        public bool IsNew { get; set; } = false;

        [Display(Name = "Món tuổi thơ")]
        public bool IsChildhoodDish { get; set; } = false;

        [Display(Name = "Còn hàng")]
        public bool IsAvailable { get; set; } = true;

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // FK → Category
        [Required]
        [Display(Name = "Danh mục")]
        public int CategoryID { get; set; }

        [ForeignKey(nameof(CategoryID))]
        public Category? Category { get; set; }
    }
}
