using System.Diagnostics;
using foldingGate.Models;
using Microsoft.AspNetCore.Mvc;

namespace foldingGate.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var produk = _context.products.ToList();
            return View(produk);
        }
    }
}
