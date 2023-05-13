using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBanDienThoai.Models;
using QuanLyBanDienThoai.ModelViews;

namespace QuanLyBanDienThoai.Controllers
{
    public class DonHangController : Controller
    {
        private readonly QlbanDienThoaiContext _context;
        public DonHangController(QlbanDienThoaiContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Details(int ? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            try
            {
                var taikhoanID = HttpContext.Session.GetString("CustomerId");
                if (string.IsNullOrEmpty(taikhoanID))
                {
                    return RedirectToAction("Login", "Accounts");
                }
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(m => m.CustomerId == Convert.ToInt32(taikhoanID));
                if (khachhang == null)
                {
                    return NotFound();
                }
                var donhang = await _context.Orders.Include(m => m.TransactStatus).FirstOrDefaultAsync(m => m.OrderId == id && Convert.ToInt32(taikhoanID) == m.CustomerId);
                if (donhang == null)
                {
                    return NotFound();
                }
                var chitietdonhang = _context.OrderDetails.Include(m => m.ProductId).AsNoTracking().Where(m => m.OrderId == id).OrderBy(m => m.OrderDetailId).ToList();
                XemDonHang donHang = new XemDonHang();
                donHang.DonHang = donhang;
                donHang.ChiTietDonHang = chitietdonhang;
                return PartialView("Details", donhang);
            }
            catch (Exception)
            {
                return NotFound();
            }

        }
    }
}
