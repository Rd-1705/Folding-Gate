using foldingGate.Models;
using foldingGate.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace foldingGate.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(int productId, int qty, Customer customer)
        {
            // 1. Simpan Data Customer
            _context.customers.Add(customer);
            _context.SaveChanges();

            // 2. Cari Produk & Hitung Total
            var product = _context.products.Find(productId);
            if (product == null) return NotFound();
            decimal totalHarga = product.Harga * qty;

            // 3. Simpan Data Order Utama
            Order order = new Order
            {
                CustomerId = customer.CustomerId,
                TanggalPesan = DateTime.Now,
                Status = "Menunggu Pembayaran",
                TotalHarga = totalHarga
            };
            _context.orders.Add(order);
            _context.SaveChanges();

            // 4. Simpan Detail Order
            OrderDetail detail = new OrderDetail
            {
                OrderId = order.OrderId,
                ProductId = productId,
                Qty = qty,
                Harga = product.Harga
            };
            _context.orderDetails.Add(detail);
            _context.SaveChanges();

            // ============================================================
            // PROSES MIDTRANS (MANUAL API - TANPA LIBRARY)
            // ============================================================
            using (var client = new HttpClient())
            {
                // Masukkan SERVER KEY Sandbox Anda di sini
                var serverKey = "SB-Mid-server-xxxxxxxxxxxxxxx:";
                var base64Key = Convert.ToBase64String(Encoding.UTF8.GetBytes(serverKey));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Key);

                var payload = new
                {
                    transaction_details = new
                    {
                        order_id = "ORDER-" + order.OrderId + "-" + DateTime.Now.Ticks,
                        gross_amount = (int)totalHarga
                    },
                    customer_details = new
                    {
                        first_name = customer.Nama,
                        phone = customer.NoHP 
                    }
                };

                var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://app.sandbox.midtrans.com/snap/v1/transactions", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(jsonResponse);

                    // Simpan SnapToken ke Database
                    order.SnapToken = result.token;
                    _context.SaveChanges();

                    return RedirectToAction("Payment", new { id = order.OrderId });
                }
            }

            // Jika gagal Midtrans, tetap ke halaman sukses tapi infokan bayar manual
            return RedirectToAction("Success");
        }

        public IActionResult Payment(int id)
        {
            var order = _context.orders.Find(id);
            if (order == null || string.IsNullOrEmpty(order.SnapToken))
                return RedirectToAction("Index", "Product");

            return View(order);
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}