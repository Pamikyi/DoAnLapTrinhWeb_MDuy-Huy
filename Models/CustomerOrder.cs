using DoAnLapTrinhWebBanThucAnNhanh.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnLapTrinhWebBanThucAnNhanh.Models
{
    [Table("CustomerOrders")]
    public class CustomerOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerOrdersID { get; set; }

        public int? UserID { get; set; }

        [ForeignKey(nameof(UserID))]
        public UserHL? User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        // Thông tin giao hàng
        [Required, StringLength(50)]
        public string ReceiverName { get; set; } = string.Empty;

        [Required, StringLength(15)]
        public string Phone { get; set; } = string.Empty;

        [Required, StringLength(200)]
        public string ShippingAddress { get; set; } = string.Empty;

        // Trạng thái đơn hàng
        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        // Phương thức thanh toán
        [StringLength(20)]
        public string PaymentMethod { get; set; } = "COD";

        [StringLength(500)]
        public string? Request { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
            = new List<OrderDetail>();
    }
}
