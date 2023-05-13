using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyBanDienThoai.Extension;
using QuanLyBanDienThoai.Helper;
using QuanLyBanDienThoai.Models;
using QuanLyBanDienThoai.Areas.Admin.Models;

namespace QuanLyBanDienThoai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminAccountsController : Controller
    {
            private readonly QlbanDienThoaiContext _context;

            public AdminAccountsController(QlbanDienThoaiContext context)
            {
                _context = context;
            }

            // GET: Admin/AdminAccounts
            public async Task<IActionResult> Index()
            {
                ViewData["QuyenTruyCap"] = new SelectList(_context.Roles, "RoleId", "Description");
                //get trang thai
                List<SelectListItem> isTrangThai = new List<SelectListItem>();
                isTrangThai.Add(new SelectListItem { Text = "Active", Value = "1" });
                isTrangThai.Add(new SelectListItem { Text = "Block", Value = "0" });
                ViewData["TrangThai"] = isTrangThai;
                var qlbanDienThoaiContext = _context.Accounts.Include(a => a.Role);
                return View(await qlbanDienThoaiContext.ToListAsync());
            }

            // GET: Admin/AdminAccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Admin/AdminAccounts/Create
        public IActionResult Create()
        {
            ViewData["QuyenTruyCap"] = new SelectList(_context.Roles, "RoleId", "RoleName");
            return View();
        }
        public IActionResult ChangePassword()
        {

            ViewData["QuyenTruyCap"] = new SelectList(_context.Roles, "RoleId", "RoleName");
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordVM vm)
        {
            if (ModelState.IsValid)
            {
                var account = _context.Accounts.AsNoTracking().SingleOrDefault(x => x.Email == vm.Email);
                if (account == null) return RedirectToAction("Login", "Account");
                var pass = (vm.PasswordNow.Trim() + account.Salt.Trim()).ToHMD5();
                if (pass == account.Password)
                {
                    string passNew = (vm.Password.Trim() + account.Salt.Trim()).ToHMD5();
                    account.Password = passNew;
                    account.LastLogin = DateTime.Now;
                    _context.Update(account);
                    _context.SaveChanges();
                    return RedirectToAction("Login", "Account", new { Area = "Admin" });
                }
            }
            return View(vm);
        }

        // POST: Admin/AdminAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,Phone,Email,Salt,Active,FullName,RoleId,LastLogin,CreateDate,Password")] Account account)
        {
            if (ModelState.IsValid)
            {
                string salt = Utilities.GetRandomKey();
                account.Salt = salt;
                account.Password = (account.Phone + salt.Trim()).ToHMD5();
                account.CreateDate = DateTime.Now;
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuyenTruyCap"] = new SelectList(_context.Roles, "RoleId", "RoleName", account.RoleId);
            return View(account);
        }

        // GET: Admin/AdminAccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["QuyenTruyCap"] = new SelectList(_context.Roles, "RoleId", "RoleName", account.RoleId);
            return View(account);
        }

        // POST: Admin/AdminAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,Phone,Email,Salt,Active,FullName,RoleId,LastLogin,CreateDate,Password")] Account account)
        {
            if (id != account.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuyenTruyCap"] = new SelectList(_context.Roles, "RoleId", "RoleName", account.RoleId);
            return View(account);
        }

        // GET: Admin/AdminAccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Admin/AdminAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Accounts == null)
            {
                return Problem("Entity set 'QlbanDienThoaiContext.Accounts'  is null.");
            }
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return (_context.Accounts?.Any(e => e.AccountId == id)).GetValueOrDefault();
        }
    }
}
