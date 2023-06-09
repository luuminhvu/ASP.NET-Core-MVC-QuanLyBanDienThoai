﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using PagedList.Core.Mvc;
using QuanLyBanDienThoai.Models;

namespace QuanLyBanDienThoai.Controllers
{
    public class ProductController : Controller
    {
        private readonly QlbanDienThoaiContext _context;
        public ProductController(QlbanDienThoaiContext context)
        {
            _context = context;
        }
        [Route("/shop.html", Name = "ShopProducts")]
        public IActionResult Index(int ? page)
        {
            try
            {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 9;
            var lstProducts = _context.Products.AsNoTracking().OrderByDescending(x => x.DateCreated);
            PagedList<Product> models = new PagedList<Product>(lstProducts, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [Route("/{Alias}", Name = "ListProducts")]
        public IActionResult List(string Alias,int page = 1)
        {
            try
            {
                var pageSize = 9;
                var danhmuc = _context.Categories.AsNoTracking().SingleOrDefault(x => x.Alias == Alias);
                var lstProducts = _context.Products.AsNoTracking().Where(x => x.CatId == danhmuc.CatId).OrderByDescending(x => x.DateCreated);
                PagedList<Product> models = new PagedList<Product>(lstProducts, page, pageSize);
                ViewBag.CurrentPage = page;
                ViewBag.CurrentCat = danhmuc;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [Route("/{Alias}-{id}.html", Name = "ProductDetails")]
        public IActionResult Details(int id)
        {
            try
            {
                var product = _context.Products.Include(x => x.Cat).FirstOrDefault(x => x.ProductId == id);
                var lsProduct  = _context.Products.AsNoTracking().Where(x => x.CatId == product.CatId && x.ProductId != id && product.Active==true).Take(4).OrderByDescending(x => x.DateCreated).ToList();
                if (product == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.lsProduct = lsProduct;
                    return View(product);
                }
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}
