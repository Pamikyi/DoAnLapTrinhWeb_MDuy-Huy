using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Category
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CategoryID { get; set; }   // Tự tăng trong DB

    [Required(ErrorMessage = "Tên danh mục không được để trống")]
    [StringLength(20, ErrorMessage = "Tên danh mục tối đa 20 ký tự")]
    public string CategoryName { get; set; } = string.Empty;  // NOT NULL + UNIQUE
}
