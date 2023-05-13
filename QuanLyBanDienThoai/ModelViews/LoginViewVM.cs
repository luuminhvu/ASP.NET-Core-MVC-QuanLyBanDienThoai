using System.ComponentModel.DataAnnotations;

namespace QuanLyBanDienThoai.ModelViews
{
    public class LoginViewVM
    {
        [Key]
        [MaxLength(100)]
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [Display(Name ="Email")]
        public string UserName { get; set; }

        [Display(Name ="Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MinLength(5,ErrorMessage ="Bạn cần dặt lớn hơn 5 kí tự")]
        public string Password { get; set; }

    }
}
