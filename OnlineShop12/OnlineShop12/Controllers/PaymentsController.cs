using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop12.Data;
using OnlineShop12.Models;

namespace OnlineShop12.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public PaymentsController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            _db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult PlaceOrder([FromForm] Payment payment)
        {
            var userId = _userManager.GetUserId(User);
            // Găsește comanda "In cos" a utilizatorului
            var cartOrder = _db.Orders
                .Include(o => o.OrderProducts)
                .FirstOrDefault(o => o.UserId == userId && o.Status == "In cos");

            payment.Id_Order = cartOrder.Id_Order;
            payment.Order_Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (cartOrder == null)
                {
                    return RedirectToAction("Index");
                }

                // Schimbă statusul comenzii
                cartOrder.Status = "Plasat";

                // Salvează modificările
                _db.Payments.Add(payment);
                _db.SaveChanges();
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.Write(error.ErrorMessage);
                }

                return RedirectToAction("MyOrders", "Orders");
            }
            else
            {
                Console.Write("Pe else");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.Write(error.ErrorMessage);
                }
                return View("Index");
            }
        }

    }
}
