using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Đồ_Án_Lập_Trình_Web_Bán_Thức_Ăn_Nhanh.Models
{
    [Table("CustomerOrders")] // 🔹 Đảm bảo EF map đúng bảng SQL
    public class CustomerOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // IDENTITY(1,1)
        [Display(Name = "Mã đơn hàng")]
        public int CustomerOrdersID { get; set; }

        [Required]
        [StringLength(7)]
        [Display(Name = "Mã người dùng")]
        public string UserID { get; set; } = string.Empty;

        [ForeignKey("UserID")]
        public virtual UserHL User { get; set; } // 🔹 Liên kết 1-1 đến bảng UserHL

        [Display(Name = "Ngày đặt hàng")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền phải >= 0")]
        [Display(Name = "Tổng tiền")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Yêu cầu thêm của khách hàng")]
        public string? Request { get; set; }

        // 🔹 Quan hệ 1-n với OrderDetails
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
