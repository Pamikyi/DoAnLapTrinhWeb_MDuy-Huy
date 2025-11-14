using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Đồ_Án_Lập_Trình_Web_Bán_Thức_Ăn_Nhanh.Models
{
    [Table("Reports")] // 🔹 Mapping đúng với bảng trong SQL
    public class Report
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Mã báo cáo")]
        public int ReportID { get; set; }

        [Display(Name = "Ngày báo cáo")]
        public DateTime ReportDate { get; set; } = DateTime.Now;

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Tổng số đơn phải >= 0")]
        [Display(Name = "Tổng số đơn hàng")]
        public int TotalReports { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Doanh thu không được âm")]
        [Display(Name = "Tổng doanh thu (VNĐ)")]
        public decimal TotalRevenue { get; set; }

        [Required(ErrorMessage = "Người tạo không được để trống")]
        [StringLength(7, ErrorMessage = "Mã người tạo tối đa 7 ký tự")]
        [Display(Name = "Người tạo báo cáo")]
        public string CreatedBy { get; set; } = string.Empty;

        // 🔸 Khóa ngoại liên kết đến bảng UserHL
        [ForeignKey(nameof(CreatedBy))]
        [Display(Name = "Tài khoản người tạo")]
        public virtual UserHL User { get; set; }
    }
}
