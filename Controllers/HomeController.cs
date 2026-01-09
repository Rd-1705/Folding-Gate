using foldingGate.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace foldingGate.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context) { _context = context; }

        public IActionResult Index()
        {
            // Mengambil 3 produk terbaru untuk ditampilkan di beranda
            var featuredProducts = _context.products.Take(3).ToList();
            return View(featuredProducts);
        }
    }
}