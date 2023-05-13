using QuanLyBanDienThoai.Models;

namespace QuanLyBanDienThoai.ModelViews
{
    public class XemDonHang
    
    {
        public Order DonHang { get; set; }
        public List<OrderDetail> ChiTietDonHang { get; set; }
    }
}
