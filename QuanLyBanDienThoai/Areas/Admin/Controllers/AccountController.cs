using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyBanDienThoai.Extension;
using QuanLyBanDienThoai.Helper;
using QuanLyBanDienThoai.Models;

namespace QuanLyBanDienThoai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly QlbanDienThoaiContext _context;

        public AccountController(QlbanDienThoaiContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [Route("dang-nhap-admin.html", Name = "DangNhapAdmin")]
        public IActionResult Login()
        {
            return View();
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("AccountId,Phone,Email,Salt,Active,FullName,RoleId,LastLogin,CreateDate,Password")] Account account)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string salt = Utilities.GetRandomKey();
        //        account.Salt = salt;
        //        account.Password = (account.Phone + salt.Trim()).ToHMD5();
        //        account.CreateDate = DateTime.Now;
        //        _context.Add(account);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["QuyenTruyCap"] = new SelectList(_context.Roles, "RoleId", "RoleName", account.RoleId);
        //    return View(account);
        //}
        [HttpPost]
        [Route("dang-nhap-admin.html", Name = "DangNhapAdmin")]
        public async Task<IActionResult> Login([Bind("Phone,Email")] Account account)
        {
            if (ModelState.IsValid)
            {
                var acc = await _context.Accounts.FirstOrDefaultAsync(m => m.Email == account.Email);
                if (acc != null)
                {
                    if (acc.Password == (account.Phone + acc.Salt.Trim()).ToHMD5())
                    {
                        if (acc.Active == true)
                        {
                            acc.LastLogin = DateTime.Now;
                            _context.Update(acc);
                            await _context.SaveChangesAsync();
                            HttpContext.Session.Set("Account", acc);
                            return RedirectToAction("Index", "Home", new { Area = "Admin" });
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
            }
            return NotFound();
        }
             
    }
}
