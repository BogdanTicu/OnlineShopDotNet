using Microsoft.AspNetCore.Mvc;
using OnlineShop12.Models;
using OnlineShop12.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace OnlineShop12.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public OrdersController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            _db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [Authorize(Roles ="Admin,Colaborator,User")]
        public IActionResult Index()
        {

            var userId = _userManager.GetUserId(User);

            var order = _db.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefault(o => o.UserId == userId && o.Status == "In cos");

            return View(order);
        }
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            var userId = _userManager.GetUserId(User);

            // Găsește comanda "In cos" a utilizatorului
            var existingOrder = _db.Orders
                .Include(o => o.OrderProducts)
                .FirstOrDefault(o => o.UserId == userId && o.Status == "In cos");

            if (existingOrder == null)
            {
                // Creează o comandă nouă dacă nu există
                existingOrder = new Order
                {
                    UserId = userId,
                    Status = "In cos",
                    Total_Amount = 0,
                    OrderProducts = new List<OrderProduct>()
                };
                _db.Orders.Add(existingOrder);
            }

            // Găsește produsul
            var product = _db.Products.FirstOrDefault(p => p.Id_Product == productId);

            if (product == null || quantity <= 0 || product.Stock < quantity)
            {
                return RedirectToAction("ProductDetails", new { id = productId });
            }

            // Verifică dacă produsul există deja în comandă
            var orderProduct = existingOrder.OrderProducts
                .FirstOrDefault(op => op.Id_Product == productId);

            if (orderProduct != null)
            {
                // Actualizează cantitatea
                orderProduct.Quantity += quantity;
            }
            else
            {
                // Adaugă produsul la comandă
                orderProduct = new OrderProduct
                {
                    Id_Product = productId,
                    Quantity = quantity,
                    Id_Order = existingOrder.Id_Order
                };
                existingOrder.OrderProducts.Add(orderProduct);
            }

            // Actualizează suma totală
            existingOrder.Total_Amount += product.Price * quantity;

            // Scade din stocul produsului
            product.Stock -= quantity;

            // Salvează modificările
            _db.SaveChanges();

            return RedirectToAction("Index","Products");
        }

        [Authorize]
        
        public IActionResult MyOrders()
        {
            var userId = _userManager.GetUserId(User);
            var orders = _db.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .Where(o => o.UserId == userId && o.Status == "Plasat")
                .ToList();

            if (orders == null)
            {
                return NotFound("No orders found.");
            }

            return View(orders);
        }
        [HttpPost]
        public IActionResult PlaceOrder(Payment payment)
        {
            var userId = _userManager.GetUserId(User);

            // Găsește comanda "In cos" a utilizatorului
            var cartOrder = _db.Orders
                .Include(o => o.OrderProducts)
                .FirstOrDefault(o => o.UserId == userId && o.Status == "In cos");

            payment.Id_Order = cartOrder.Id_Order;
            payment.Order_Date = DateTime.Now;
            if (cartOrder == null)
            {
                return RedirectToAction("Index");
            }

            // Schimbă statusul comenzii
            cartOrder.Status = "Plasat";

            // Salvează modificările
            _db.Payments.Add(payment);
            _db.SaveChanges();

            return RedirectToAction("MyOrders","Orders");
        }
        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var userId = _userManager.GetUserId(User);
            Order order = _db.Orders.Include(o => o.OrderProducts)
                           .First(o => o.UserId == userId && o.Status == "In cos");
            Product product = _db.Products.Find(productId);
            if (product != null)
            {
                OrderProduct pr = _db.OrderProducts.Find(order.Id_Order, productId);
                product.Stock += pr.Quantity;
                order.Total_Amount -= product.Price * pr.Quantity;
                _db.Remove(pr);
                _db.SaveChanges();
            }
            return RedirectToAction("Index", "Orders");
        }
        
    }
}
