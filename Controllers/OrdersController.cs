using foldingGate.Models;
using foldingGate.Models.DB;
using Microsoft.AspNetCore.Mvc;

namespace foldingGate.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Create(int productId)
        {
            ViewBag.Product = _context.products.Find(productId);
            return View();
        }

        [HttpPost]
        public IActionResult Create(int productId, int qty, Customer customer)
        {
            _context.customers.Add(customer);
            _context.SaveChanges();

            var product = _context.products.Find(productId);

            Order order = new Order
            {
                CustomerId = customer.CustomerId,
                TanggalPesan = DateTime.Now,
                Status = "Menunggu Konfirmasi",
                TotalHarga = product.Harga * qty
            };

            _context.orders.Add(order);
            _context.SaveChanges();

            OrderDetail detail = new OrderDetail
            {
                OrderId = order.OrderId,
                ProductId = productId,
                Qty = qty,
                Harga = product.Harga
            };

            _context.orderDetails.Add(detail);
            _context.SaveChanges();

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
