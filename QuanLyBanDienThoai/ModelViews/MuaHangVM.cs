using QuanLyBanDienThoai.Models;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBanDienThoai.ModelViews
{
    public class MuaHangVM
    {
        [Key]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "vui lòng nhập họ và tên")]
        public string FullName { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string Phone { get;set;}
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string Address { get; set; }
        public int TinhThanh { get; set; }
        public int QuanHuyen { get; set; }
        public int PhuongXa { get; set; }
        public int PaymentID { get; set; }
        public string Note { get; set; }

    }
}
