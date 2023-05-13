using Microsoft.AspNetCore.Mvc;
using QuanLyBanDienThoai.Extension;
using QuanLyBanDienThoai.ModelViews;

namespace QuanLyBanDienThoai.Controllers.Components
{
    public class HeaderCartViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            return View(cart);
        }
    }
}
