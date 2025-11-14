using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Đồ_Án_Lập_Trình_Web_Bán_Thức_Ăn_Nhanh.Models
{
    [Table("Roles")] // 🔹 Mapping đúng với bảng Roles trong SQL
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Mã vai trò")]
        public int RoleID { get; set; }

        [Required(ErrorMessage = "Tên vai trò không được để trống")]
        [StringLength(20, ErrorMessage = "Tên vai trò tối đa 20 ký tự")]
        [Display(Name = "Tên vai trò")]
        public string RoleName { get; set; } = string.Empty;

        // 🔸 Liên kết 1-nhiều với UserHL
        [Display(Name = "Danh sách người dùng")]
        public virtual ICollection<UserHL> Users { get; set; } = new HashSet<UserHL>();
    }
}
