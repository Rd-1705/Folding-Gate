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
        public async Task<IActionResult> Create(Product product, IFormFile uploadFoto)
        {
            if (uploadFoto != null && uploadFoto.Length > 0)
            {
                // Tentukan path penyimpanan
                string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
                string fileName = Guid.NewGuid().ToString() + "_" + uploadFoto.FileName;
                string filePath = Path.Combine(folder, fileName);

                // Simpan file ke folder
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadFoto.CopyToAsync(stream);
                }

                // Simpan nama file ke properti GambarUrl
                product.Gambar = fileName;
            }

            _context.products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
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