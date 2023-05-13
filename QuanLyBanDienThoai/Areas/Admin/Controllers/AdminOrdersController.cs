using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using QuanLyBanDienThoai.Models;

namespace QuanLyMobile.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminOrdersController : Controller
    {
        private readonly QlbanDienThoaiContext _context;

        public AdminOrdersController(QlbanDienThoaiContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminOrders
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lstOrders = _context.Orders.Include(x=>x.TransactStatus).Include(x=>x.Customer).AsNoTracking().OrderByDescending(x => x.OrderDate);
            PagedList<Order> models = new PagedList<Order>(lstOrders, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: Admin/AdminOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.TransactStatus)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            var chitietdonhang = _context.OrderDetails.Include(x=>x.Product).AsNoTracking().Where(x => x.OrderId == order.OrderId).OrderBy(x => x.OrderDetailId).ToList();
            ViewBag.chitietdonhang = chitietdonhang;
            return View(order);
        }
        public async Task<IActionResult> ChangeStatus(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.Include(x=>x.Customer).Include(x=>x.TransactStatus).AsNoTracking().FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
  
            ViewData["TrangThai"] = new SelectList(_context.TransactStatuses, "TransactStatusId", "Status", order.TransactStatusId);
            return PartialView("ChangeStatus", order);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id, [Bind("OrderId,CustomerId,OrderDate,ShipDate,TransactStatusId,Deleted,Paid,PaymentDate,PaymentId,Note,TotalMoney,Address,District,Ward,LocationId")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var donhang = await _context.Orders.Include(x => x.Customer).Include(x => x.TransactStatus).FirstOrDefaultAsync(x => x.OrderId == id);
                    if (donhang != null)
                    {
                        donhang.Paid = order.Paid;
                        donhang.TransactStatusId = order.TransactStatusId;
                        donhang.Deleted = order.Deleted;
                        if (donhang.Paid == true)
                        {
                            donhang.PaymentDate = DateTime.Now;
                        }
                        if (donhang.TransactStatusId == 5)
                        {
                            donhang.Deleted = true;
                        }
                        _context.Update(donhang);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", order.CustomerId);
            ViewData["TrangThai"] = new SelectList(_context.TransactStatuses, "TransactStatusId", "Status", order.TransactStatusId);
            return PartialView("ChangeStatus", order);

        }

        // GET: Admin/AdminOrders/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            ViewData["TransactStatusId"] = new SelectList(_context.TransactStatuses, "TransactStatusId", "TransactStatusId");
            return View();
        }

        // POST: Admin/AdminOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,CustomerId,OrderDate,ShipDate,TransactStatusId,Deleted,Paid,PaymentDate,PaymentId,Note,TotalMoney,Address,District,Ward,LocationId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", order.CustomerId);
            ViewData["TransactStatusId"] = new SelectList(_context.TransactStatuses, "TransactStatusId", "TransactStatusId", order.TransactStatusId);
            return View(order);
        }

        // GET: Admin/AdminOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", order.CustomerId);
            ViewData["TrangThai"] = new SelectList(_context.TransactStatuses, "TransactStatusId", "Status", order.TransactStatusId);
            ViewData["Location"] = new SelectList(_context.Locations, "LocationId", "Name", order.LocationId);
            return View(order);
        }

        // POST: Admin/AdminOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerId,OrderDate,ShipDate,TransactStatusId,Deleted,Paid,PaymentDate,PaymentId,Note,TotalMoney,Address,District,Ward,LocationId")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", order.CustomerId);
            ViewData["TrangThai"] = new SelectList(_context.TransactStatuses, "TransactStatusId", "Status", order.TransactStatusId);
            return View(order);
        }

        // GET: Admin/AdminOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.TransactStatus)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Admin/AdminOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'QlbanDienThoaiContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
    }
}
