using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Đồ_Án_Lập_Trình_Web_Bán_Thức_Ăn_Nhanh.Models
{
    [Table("UserHL")] // 🔹 Mapping đúng với bảng trong SQL
    public class UserHL
    {
        [Key]
        [StringLength(7, ErrorMessage = "Mã người dùng tối đa 7 ký tự")]
        [Display(Name = "Mã người dùng")]
        public string UserID { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(100, ErrorMessage = "Tên đăng nhập tối đa 100 ký tự")]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(100, ErrorMessage = "Mật khẩu tối đa 100 ký tự")]
        [Display(Name = "Mật khẩu")]
        public string Passwords { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được để trống")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [StringLength(15)]
        [RegularExpression(@"^[0-9+]+$", ErrorMessage = "Số điện thoại chỉ được chứa số và dấu '+'")]
        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }

        // 🔸 Liên kết với Role
        [Display(Name = "Mã vai trò")]
        public int RoleID { get; set; }

        [ForeignKey(nameof(RoleID))]
        [Display(Name = "Vai trò")]
        public virtual Role Role { get; set; }

        // 🔹 Liên kết 1-nhiều với CustomerOrder
        [Display(Name = "Danh sách đơn hàng")]
        public virtual ICollection<CustomerOrder> CustomerOrders { get; set; } = new HashSet<CustomerOrder>();

        // 🔹 Liên kết 1-nhiều với Report
        [Display(Name = "Danh sách báo cáo")]
        public virtual ICollection<Report> Reports { get; set; } = new HashSet<Report>();
    }
}
