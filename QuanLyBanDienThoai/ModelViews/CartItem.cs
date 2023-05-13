using QuanLyBanDienThoai.Models;

namespace QuanLyBanDienThoai.ModelViews
{
    public class CartItem
    {
        public Product product { get; set; }
        public int amount { get; set; }
        public double Total => amount * product.Price.Value;

    }
}
