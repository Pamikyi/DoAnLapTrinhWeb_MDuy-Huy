using System;
using System.ComponentModel.DataAnnotations;

namespace Đồ_Án_Lập_Trình_Web_Bán_Thức_Ăn_Nhanh.Models
{
    public class Contacts
    {
        [Key]
        [StringLength(5)]
        public string ContactID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [Display(Name = "Họ và tên")]
        public string Fullname { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung phản hồi")]
        [Display(Name = "Nội dung phản hồi")]
        public string MessageContent { get; set; } 

        [Display(Name = "Ngày gửi")]
        public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}
