﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using QuanLyBanDienThoai.Models;

namespace QuanLyBanDienThoai.Controllers
{
    public class PageController : Controller
    {
        private readonly QlbanDienThoaiContext _context;
        public PageController(QlbanDienThoaiContext context)
        {
            _context = context;
        }
        [Route("/page/{Alias}", Name = "PageDetails")]
        public IActionResult Details(string Alias)
        {
            if (string.IsNullOrEmpty(Alias)) return RedirectToAction("Index", "Home");
            var page = _context.Pages.AsNoTracking().SingleOrDefault(x => x.Alias == Alias);
            if (page == null)
            {
                return RedirectToAction("Index","Home");
            }
            return View(page); 
        }
    }
}
