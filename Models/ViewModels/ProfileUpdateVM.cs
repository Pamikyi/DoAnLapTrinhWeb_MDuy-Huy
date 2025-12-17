using System.ComponentModel.DataAnnotations;

namespace DoAnLapTrinhWebBanThucAnNhanh.ViewModels
{
    public class ProfileUpdateVM
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;

        [StringLength(15)]
        public string? PhoneNumber { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }
    }
}
