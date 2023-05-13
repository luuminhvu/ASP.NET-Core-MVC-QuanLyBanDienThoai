using System.ComponentModel.DataAnnotations;

namespace QuanLyBanDienThoai.ModelViews
{
    public class MuaHangSuccess
    {
        public int DonHangID { get; set; }
        [Required(ErrorMessage = "vui lòng nhập họ và tên")]
        public string FullName { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tỉnh thành")]
        public int TinhThanh { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập quận huyện")]
        public int QuanHuyen { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập phường xã")]
        public int PhuongXa { get; set; }
        public int PaymentID { get; set; }
        public string Note { get; set; }
    }
}
