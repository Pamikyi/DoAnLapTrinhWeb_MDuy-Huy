using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Đồ_Án_Lập_Trình_Web_Bán_Thức_Ăn_Nhanh.Models
{
    [Table("OrderDetails")] // 🔹 Đảm bảo mapping đúng với bảng SQL
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Mã chi tiết đơn hàng")]
        public int OrderDetailID { get; set; }

        [Required]
        [Display(Name = "Mã đơn hàng")]
        public int CustomerOrderID { get; set; }

        [ForeignKey(nameof(CustomerOrderID))]
        public virtual CustomerOrder CustomerOrder { get; set; }

        [Required]
        [StringLength(5)]
        [Display(Name = "Mã sản phẩm")]
        public string ProductID { get; set; } = string.Empty;

        [ForeignKey(nameof(ProductID))]
        public virtual required Product Product { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá không được âm")]
        [Display(Name = "Giá bán")]
        public decimal Price { get; set; }
    }
}
