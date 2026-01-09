using foldingGate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace foldingGate.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            ViewBag.TotalOrder = _context.orders.Count();
            ViewBag.TotalProduk = _context.products.Count();
            return View();
        }

        public IActionResult Orders()
        {
            var orders = _context.orders
                .Include(o => o.Customer)
                .ToList();
            return View(orders);
        }

        public IActionResult UpdateStatus(int id, string status)
        {
            var order = _context.orders.Find(id);
            order.Status = status;
            _context.SaveChanges();
            return RedirectToAction("Orders");
        }

        public IActionResult Laporan()
        {
            // Mengambil pesanan yang sudah selesai/terbayar
            var laporan = _context.orders
                .Where(o => o.Status == "Selesai")
                .Join(_context.customers, o => o.CustomerId, c => c.CustomerId, (o, c) => new { o, c })
                .ToList();

            ViewBag.TotalPendapatan = laporan.Sum(x => x.o.TotalHarga);
            return View(laporan);
        }
    }
}
