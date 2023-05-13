using Microsoft.AspNetCore.Mvc;
using QuanLyBanDienThoai.Extension;
using QuanLyBanDienThoai.ModelViews;

namespace QuanLyBanDienThoai.Controllers.Components
{
    public class NumberCartViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {       
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            //int SoLuongSanPham = 0;
            //if (cart != null)
            //{
            //    SoLuongSanPham = cart.Count();
            //}
            return View(cart);
        }
    }
}
