using foldingGate.Models;
using foldingGate.Models.DB;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace foldingGate.Controllers
{
    public class ProductAdminController : Controller
    {
        private readonly AppDbContext _context;
        public ProductAdminController(AppDbContext context) { _context = context; }

        public IActionResult Index() => View(_context.products.ToList());

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public IActionResult Edit(int id) => View(_context.products.Find(id));

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            _context.products.Update(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var product = _context.products.Find(id);
            _context.products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}