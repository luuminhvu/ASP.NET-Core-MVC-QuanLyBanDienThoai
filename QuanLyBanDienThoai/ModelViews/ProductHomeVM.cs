using QuanLyBanDienThoai.Models;

namespace QuanLyBanDienThoai.ModelViews
{
    public class ProductHomeVM
    {
        public Category category { get; set; }
        public List<Product> lsproducts { get; set; }
    }
}
