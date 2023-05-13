using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using PagedList.Core.Mvc;
using QuanLyBanDienThoai.Models;

namespace QuanLyBanDienThoai.Controllers
{
    public class BlogController : Controller
    {

        private readonly QlbanDienThoaiContext _context;
        public BlogController(QlbanDienThoaiContext context)
        {
            _context = context;
        }
        [Route("index.html",Name ="Blog")]
        public IActionResult Index(int? page)
        {

            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var lstPosts = _context.Posts.AsNoTracking().OrderByDescending(x => x.PostId);
            PagedList<Post> models = new PagedList<Post>(lstPosts, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }
        [Route("/post/{Alias}-{id}.html",Name ="BlogDetails")]
        public IActionResult Details(int id)
        {
            var post = _context.Posts.AsNoTracking().SingleOrDefault(x => x.PostId == id);
            if (post == null)
            {
                return RedirectToAction("Index");
            }
            var lsBaiVietLienQuan = _context.Posts.AsNoTracking().Where(x=>x.Published == true && x.PostId != id).Take(3).OrderByDescending(x=>x.CreateDate).ToList();
            ViewBag.lsBaiVietLienQuan = lsBaiVietLienQuan;
            return View(post);
        }
    }
}
