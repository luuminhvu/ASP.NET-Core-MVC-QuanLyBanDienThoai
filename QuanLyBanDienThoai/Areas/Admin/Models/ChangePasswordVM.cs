using System.ComponentModel.DataAnnotations;

namespace QuanLyBanDienThoai.Areas.Admin.Models
{
    public class ChangePasswordVM
    {
        [Key]
        public int AccountId { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Mật khẩu cũ")]
        public string PasswordNow { get; set; }
        [Display(Name = "Mật khẩu mới")]
        [Required(ErrorMessage = "Mật khẩu mới không được để trống")]
        [MinLength(5, ErrorMessage = "Mật khẩu mới phải ít nhất 5 ký tự")]
        public string Password { get; set; }
        [Display(Name = "Nhập lại mật khẩu mới")]
        [Compare("Password", ErrorMessage = "Mật khẩu nhập lại không khớp")]
        public string ConfirmPassword { get; set; }
    }
}
