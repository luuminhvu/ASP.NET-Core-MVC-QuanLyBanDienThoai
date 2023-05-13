using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBanDienThoai.ModelViews
{
    public class RegisterVM
    {
        [Key]
        public int CustomerId { get; set; }
        [Display(Name ="Họ và Tên")]
        [Required(ErrorMessage = "vui lòng nhập họ và tên")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        [Remote(action:"ValidateEmail",controller:"Accounts")]
        public string Email { get; set; }
        [MaxLength(11)]
        [Required(ErrorMessage ="Vui lòng nhập số điện thoại")]
        [Display(Name="Điện thoại")]
        [DataType(DataType.PhoneNumber)]
        [Remote(action:"ValidatePhone",controller:"Accounts")]
        public string Phone { get; set; }
        [Display(Name = "Mật Khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MinLength(5, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 5 kí tự")]
        public string Password { get; set; }
        [Display(Name = "Nhập lại Mật Khẩu")]
        [MinLength(5, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 5 kí tự")]
        [Compare("Password",ErrorMessage = "Vui lòng nhập mật khẩu giống nhau")]
        public string ConfirmPassword { get; set; }
    }
}
