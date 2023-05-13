using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBanDienThoai.Extension;
using QuanLyBanDienThoai.Helper;
using QuanLyBanDienThoai.Models;
using QuanLyBanDienThoai.ModelViews;
using System.Security.Claims;
using XAct.Security;

namespace QuanLyBanDienThoai.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        private readonly QlbanDienThoaiContext _context;
        public AccountsController(QlbanDienThoaiContext context)
        {
            _context = context;
        }
      
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("tai-khoan-cua-toi.html", Name = "Dashboard")]
        public IActionResult Dashboard()
        {
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            if (taikhoanID != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                if (khachhang != null)
                {
                    var lsDonHang = _context.Orders.Include(x=>x.TransactStatus).AsNoTracking().Where(x => x.CustomerId == khachhang.CustomerId).OrderByDescending(x => x.OrderDate).ToList();
                    ViewBag.lsDonHang = lsDonHang;
                    return View(khachhang);
                }
            }
            return RedirectToAction("Login");
        }
        [HttpGet]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult ValidatePhone(string Phone)
        {
            try
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x=>x.Phone.ToLower()==Phone.ToLower());
                if(khachhang !=null)
                    return Json(data: $"Số điện thoại {Phone} đã tồn tại");
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }
        [HttpGet]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult ValidateEmail(string Email)
        {
            try
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Email.ToLower() == Email.ToLower());
                if (khachhang != null)
                    return Json(data: $"Email {Email} đã tồn tại");
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }
        [HttpGet]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [Route("dang-ky.html", Name = "DangKy")]
        public IActionResult DangKyTaiKhoan()
        {
            return View();
        }
        [HttpPost]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [Route("dang-ky.html",Name = "DangKy")]
        public async Task<IActionResult> DangKyTaiKhoan(RegisterVM taikhoan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string salt = Utilities.GetRandomKey();
                    Customer khachhang = new Customer
                    {
                        FullName = taikhoan.FullName,
                        Phone = taikhoan.Phone.Trim().ToLower(),
                        Email = taikhoan.Email.Trim().ToLower(),
                        Password = (taikhoan.Password + salt.Trim()).ToHMD5(),
                        Active = true,
                        Salt = salt,
                        CreateDate = DateTime.Now
                    };
                    try
                    {
                        _context.Add(khachhang);
                        await _context.SaveChangesAsync();
                        HttpContext.Session.SetString("CustomerId", khachhang.CustomerId.ToString());
                        var taikhoanID = HttpContext.Session.GetString("CustomerId");
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,khachhang.FullName),
                            new Claim("CustomerId",khachhang.CustomerId.ToString())
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        return RedirectToAction("Dashboard", "Accounts");
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("DangKyTaiKhoan", "Accounts");
                    }
                }
                else
                {
                    return View(taikhoan);
                }
            }
            catch
            {
                return View(taikhoan);
            }
        }
        [HttpGet]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [Route("dang-nhap.html", Name = "DangNhap")]
        public IActionResult Login(string returnUrl = null)
        {
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            if (taikhoanID != null)
            {
              
                return RedirectToAction("Dashboard", "Accounts");
            }
            return View();
        }
        [HttpPost]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [Route("dang-nhap.html", Name = "DangNhap")]
        public async Task<IActionResult>Login(LoginViewVM customer,string returnUrl = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isEmail = Utilities.IsValidEmail(customer.UserName);
                    if (!isEmail) return View(customer);
                    var khachhang = _context.Customers.AsNoTracking().SingleOrDefaultAsync(x => x.Email.Trim() == customer.UserName);
                    if (khachhang == null) return RedirectToAction("DangKyTaiKhoan");
                    string pass = (customer.Password + khachhang.Result.Salt.Trim()).ToHMD5();
                    if (khachhang.Result.Password != pass) return View(customer);
                    if(khachhang.Result.Active == false) return RedirectToAction("ThongBao","Accounts");
                    HttpContext.Session.SetString("CustomerId", khachhang.Result.CustomerId.ToString());
                    var taikhoanID = HttpContext.Session.GetString("CustomerId");
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,khachhang.Result.FullName),
                        new Claim("CustomerId",khachhang.Result.CustomerId.ToString())
                    };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    return RedirectToAction("Dashboard", "Accounts");
                }
            }
            catch
            {
                return RedirectToAction("DangKyTaiKhoan", "Accounts");
            }
            return View(customer);
        }
        [HttpGet]
        [Route("dang-xuat.html", Name = "Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("CustomerId");
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordVM model)
        {
            try
            {
                var taikhoanID = HttpContext.Session.GetString("CustomerId");
                if (taikhoanID == null) return RedirectToAction("Login", "Accounts");
                if (ModelState.IsValid)
                {
                    var taikhoan = _context.Customers.Find(Convert.ToInt32(taikhoanID));
                    if (taikhoan == null) return RedirectToAction("Login", "Accounts");
                    var pass = (model.PasswordNow + taikhoan.Salt.Trim()).ToHMD5();
                    if (pass == taikhoan.Password)
                    {
                        string passnew = (model.Password.Trim() + taikhoan.Salt.Trim()).ToHMD5();
                        taikhoan.Password = passnew;
                        _context.Update(taikhoan);
                        _context.SaveChanges();
                        return RedirectToAction("Dashboard", "Accounts");
                    }
                 
                }

            }
            catch
            {
                return RedirectToAction("Login", "Accounts");

            }
            return RedirectToAction("Login", "Accounts");
        }
        
    }
}
