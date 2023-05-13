using Microsoft.AspNetCore.Mvc;
using QuanLyBanDienThoai.Extension;
using QuanLyBanDienThoai.Models;
using QuanLyBanDienThoai.ModelViews;

namespace QuanLyBanDienThoai.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly QlbanDienThoaiContext _context;
        public ShoppingCartController(QlbanDienThoaiContext context)
        {
            _context = context;
        }
        public List<CartItem> GioHang
        {
            get
            {
                var gh = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if(gh==default(List<CartItem>))
                {
                    gh = new List<CartItem>();
                }
                return gh;
            }

        }
        [HttpPost]
        [Route("api/cart/add")]
        public IActionResult AddToCart(int productID,int ? amount)
        {
            List<CartItem> gioHang = GioHang;
            try
            {
                CartItem item = GioHang.SingleOrDefault(p => p.product.ProductId == productID);
                if (item != null)
                {
                    if (amount.HasValue)
                    {
                        item.amount = amount.Value;
                    }
                    else
                    {
                        item.amount++;
                    }
                }
                else
                {
                    Product hh = _context.Products.SingleOrDefault(p => p.ProductId == productID);
                    item = new CartItem()
                    {
                        product = hh,
                        amount = amount.HasValue ? amount.Value : 1
                    };
                    gioHang.Add(item);
                }
                HttpContext.Session.Set<List<CartItem>>("GioHang", gioHang);
                return Json(new {success = true});
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }
        [HttpPost]
        [Route("api/cart/remove")]
        public IActionResult Remove(int productID)
        {
            try
            {
                List<CartItem> gioHang = GioHang;
                CartItem item = gioHang.SingleOrDefault(p => p.product.ProductId == productID);
                if (item != null)
                {
                    gioHang.Remove(item);
                }
                HttpContext.Session.Set<List<CartItem>>("GioHang", gioHang);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
               
        }
        [Route("/cart.html",Name = "Cart")]
        public IActionResult Index()
        {
            return View(GioHang);
        }
        [HttpPost]
        [Route("api/cart/update")]
        public IActionResult UpdateCart(int productID,int ? amount)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            try
            {
                if(cart != null)
                {
                    CartItem item = cart.SingleOrDefault(p => p.product.ProductId == productID);
                    if(item != null && amount.HasValue)
                    {
                        item.amount = amount.Value;
                       
                    }
                    HttpContext.Session.Set<List<CartItem>>("GioHang", cart);
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }
    }
}
