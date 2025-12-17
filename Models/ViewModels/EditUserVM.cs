using System.ComponentModel.DataAnnotations;

namespace DoAnLapTrinhWebBanThucAnNhanh.Models.ViewModels
{
    public class EditUserVM
    {
        public int UserID { get; set; }

        public string Username { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        [Required]
        public int RoleID { get; set; }
    }
}
