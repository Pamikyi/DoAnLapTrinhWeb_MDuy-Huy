using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnLapTrinhWebBanThucAnNhanh.Models
{
    [Table("UserHL")]
    public class UserHL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Mã người dùng")]
        public int UserID { get; set; }  // ✔ CHUẨN HÓA: INT IDENTITY

        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(100, ErrorMessage = "Tên đăng nhập tối đa 100 ký tự")]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(200, ErrorMessage = "Hash mật khẩu tối đa 200 ký tự")]
        [Display(Name = "Mật khẩu (đã mã hóa)")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được để trống")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [StringLength(15)]
        [RegularExpression(@"^[0-9+]+$", ErrorMessage = "Số điện thoại chỉ được chứa số và dấu '+'")]
        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }

        // FK → Role
        [Display(Name = "Mã vai trò")]
        public int RoleID { get; set; }

        [ForeignKey(nameof(RoleID))]
        [Display(Name = "Vai trò")]
        public Role? Role { get; set; }

        [StringLength(200)]
        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }

        // 1 User → nhiều Order
        public ICollection<CustomerOrder> CustomerOrders { get; set; } = new HashSet<CustomerOrder>();

        // 1 User → nhiều Report
        public ICollection<Report> Reports { get; set; } = new HashSet<Report>();
    }
}
