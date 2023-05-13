using Microsoft.AspNetCore.Mvc;
using QuanLyBanDienThoai.Models;

namespace QuanLyBanDienThoai.Controllers
{
    public class LocationController : Controller
    {
        private readonly QlbanDienThoaiContext _context;
        public LocationController(QlbanDienThoaiContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult QuanHuyenList(int LocationId)
        {
            var QuanHuyens = _context.Locations.OrderBy(x=>x.LocationId).Where(x => x.ParentCode == LocationId && x.Levels == 2).OrderBy(x=>x.Name).ToList();
            return Json(QuanHuyens);

        }
        public IActionResult PhuongXaList(int LocationId)
        {
            var PhuongXas = _context.Locations.OrderBy(x => x.LocationId).Where(x => x.ParentCode == LocationId && x.Levels == 3).OrderBy(x => x.Name).ToList();
            return Json(PhuongXas);
        }
    }
}
