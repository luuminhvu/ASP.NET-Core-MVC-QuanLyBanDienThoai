using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBanDienThoai.Models;

namespace QuanLyMobile.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SearchController : Controller
    {
        private readonly QlbanDienThoaiContext _context;
        public SearchController(QlbanDienThoaiContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult FindProduct(string keyword)
        {
            List<Product> products = new List<Product>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListProductsSearchPartial", null);
            }
            products = _context.Products
                .AsNoTracking()
                .Include(a => a.Cat)
                .Where(x => x.ProductName.Contains(keyword))
                .OrderByDescending(x => x.ProductName)
                .Take(10)
                .ToList();
            if (products == null)
            {
                return PartialView("ListProductsSearchPartial", null);
            }
            else
            {
                return PartialView("ListProductsSearchPartial", products);
            }
        }
    }
}
