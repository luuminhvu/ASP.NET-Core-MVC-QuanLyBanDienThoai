﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyBanDienThoai.Extension;
using QuanLyBanDienThoai.Models;
using QuanLyBanDienThoai.ModelViews;
using System.Linq;

namespace QuanLyBanDienThoai.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly QlbanDienThoaiContext _context;
        public CheckoutController(QlbanDienThoaiContext context)
        {
            _context = context;
        }
        public List<CartItem> GioHang
        {
            get
            {
                var gh = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if (gh == default(List<CartItem>))
                {
                    gh = new List<CartItem>();
                }
                return gh;
            }
        }
        [Route("checkout.html", Name = "Checkout")]
        public IActionResult Index(string returnUrl = null)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            MuaHangVM model = new MuaHangVM();
            if (taikhoanID != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(p => p.CustomerId == Convert.ToInt32(taikhoanID));
                model.CustomerId = khachhang.CustomerId;
                model.FullName = khachhang.FullName;
                model.Email = khachhang.Email;
                model.Phone = khachhang.Phone;
                model.Address = khachhang.Address;
            }
            ViewData["lsTinhThanh"] = new SelectList(_context.Locations.Where(x => x.Levels == 1).OrderBy(x => x.Type).ToList(), "LocationId", "Name");
            ViewBag.GioHang = cart;
            return View(model);
        }
         
        [HttpPost]
        [Route("checkout.html", Name = "Checkout")]
        public IActionResult Index(MuaHangVM muahang)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            MuaHangVM model = new MuaHangVM();
            if (taikhoanID != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(p => p.CustomerId == Convert.ToInt32(taikhoanID));
                model.CustomerId = khachhang.CustomerId;
                model.FullName = khachhang.FullName;
                model.Email = khachhang.Email;
                model.Phone = khachhang.Phone;
                model.Address = khachhang.Address;
                //khachhang.LocationId = muahang.TinhThanh;
                //khachhang.District = muahang.QuanHuyen;
                //khachhang.Ward = muahang.PhuongXa;
                khachhang.Address = muahang.Address;
                _context.Update(khachhang);
                _context.SaveChanges();
                Order donhang = new Order();
                donhang.CustomerId = model.CustomerId;
                donhang.OrderDate = DateTime.Now;
                donhang.Address = khachhang.Address;
                donhang.LocationId = khachhang.LocationId;
                donhang.District = khachhang.District;
                donhang.Ward = khachhang.Ward;
                donhang.TransactStatusId = 1;
                donhang.Deleted = false;
                donhang.Paid = false;
                donhang.Note = model.Note;
                donhang.TotalMoney = Convert.ToInt32(cart.Sum(x => x.Total));
                _context.Add(donhang);
                _context.SaveChanges();
                foreach (var item in cart)
                {
                    OrderDetail chitietdonhang = new OrderDetail();
                    chitietdonhang.OrderId = donhang.OrderId;
                    chitietdonhang.ProductId = item.product.ProductId;
                    chitietdonhang.Quantity = item.amount;
                    chitietdonhang.Price = item.product.Price;
                    chitietdonhang.Total = donhang.TotalMoney;
                    chitietdonhang.Product = _context.Products.Find(item.product.ProductId); // Lấy thông tin sản phẩm tương ứng
                    chitietdonhang.Product.UnitslnStock = chitietdonhang.Product.UnitslnStock - item.amount;
                    chitietdonhang.CreateDate = DateTime.Now;
                    _context.Add(chitietdonhang);
                    _context.SaveChanges();
                }

                _context.SaveChanges();
                HttpContext.Session.Remove("GioHang");
                return RedirectToAction("Dashboard", "Accounts");
            }
            //try
            //{
            //    if (ModelState.IsValid)
            //    {
            //        Order donhang = new Order();
            //        donhang.CustomerId = model.CustomerId;
            //        donhang.OrderDate = DateTime.Now;
            //        donhang.Address = model.Address;
            //        donhang.LocationId = model.TinhThanh;
            //        donhang.District = model.QuanHuyen;
            //        donhang.Ward = model.PhuongXa;
            //        donhang.TransactStatusId = 1;
            //        donhang.Deleted = false;
            //        donhang.Paid = false;
            //        donhang.Note = model.Note;
            //        donhang.TotalMoney = Convert.ToInt32(cart.Sum(x => x.Total));
            //        _context.Add(donhang);
            //        _context.SaveChanges();
            //        foreach (var item in cart)
            //        {
            //            OrderDetail chitietdonhang = new OrderDetail();
            //            chitietdonhang.OrderId = donhang.OrderId;
            //            chitietdonhang.ProductId = item.product.ProductId;
            //            chitietdonhang.Quantity = item.amount;
            //            chitietdonhang.Price = item.product.Price;
            //            chitietdonhang.Total = donhang.TotalMoney;
            //            chitietdonhang.CreateDate = DateTime.Now;
            //            _context.Add(chitietdonhang);
            //            _context.SaveChanges();
            //        }
            //        _context.SaveChanges();
            //        HttpContext.Session.Remove("GioHang");
            //        return RedirectToAction("Dashboard","Accounts");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ViewData["lsTinhThanh"] = new SelectList(_context.Locations.Where(x => x.Levels == 1).OrderBy(x => x.Type).ToList(), "LocationId", "Name");
            //    ViewBag.GioHang = cart;
            //    return NotFound();

            //}
            ViewData["lsTinhThanh"] = new SelectList(_context.Locations.Where(x => x.Levels == 1).OrderBy(x => x.Type).ToList(), "LocationId", "Name");
            ViewBag.GioHang = cart;
            return View();

        }
        [Route("dat-hang-thanh-cong.html",Name= "Success")]
        public IActionResult Success()
        {
            try
            {
                var taikhoanID = HttpContext.Session.GetString("CustomerId");
                if(string.IsNullOrEmpty(taikhoanID))
                {
                    return RedirectToAction("Login", "Accounts", new { returnUrl = "/dat-hang-thanh-cong.html" });

                }
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(p => p.CustomerId == Convert.ToInt32(taikhoanID));
                var donhang = _context.Orders.Where(x=>x.CustomerId== Convert.ToInt32(taikhoanID)).OrderByDescending(x=>x.OrderDate).FirstOrDefault();
                MuaHangSuccess successVM = new MuaHangSuccess();
                successVM.FullName = khachhang.FullName;
                successVM.DonHangID = donhang.OrderId;
                successVM.Phone = khachhang.Phone;
                successVM.Address = khachhang.Address;
                //successVM.PhuongXa=GetNameLocation(donhang.Ward.Value);
                //successVM.TinhThanh = GetNameLocation(donhang.District.Value);
                return View(successVM);
            }
            catch
            {
                return View();
            }
        }

    }
}
