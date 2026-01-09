using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using foldingGate.Models;
using foldingGate.Models.DB;

namespace foldingGate.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        public AccountController(AppDbContext context) { _context = context; }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _context.users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                };
                var identity = new ClaimsIdentity(claims, "CookieAuth");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("CookieAuth", principal);
                return RedirectToAction("Dashboard", "Admin");
            }
            ViewBag.Error = "Username atau Password salah!";
            return View();
        }

        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(User newUser)
        {
            // Catatan: Data Pemilik default, hanya bisa daftar sebagai Admin
            newUser.Role = "Admin";

            if (ModelState.IsValid)
            {
                _context.users.Add(newUser);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }
    }
}