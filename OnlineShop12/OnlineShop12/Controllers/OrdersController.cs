using Microsoft.AspNetCore.Mvc;
using OnlineShop12.Models;
using OnlineShop12.Data;

namespace OnlineShop12.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _db;

        public OrdersController(ApplicationDbContext context)
        {
            _db = context;
        }
        public IActionResult Index()
        {
            var orders = from ord in _db.Orders
                             select ord;

            ViewBag.Orders = orders;

            return View();
        }
        public ActionResult Show(int id)
        {
            Order ord = _db.Orders.Find(id);

            ViewBag.Order = ord;

            return View();
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public IActionResult New(Order ord)
        {
            try
            {
                _db.Orders.Add(ord);

                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View();
            }

        }

        public IActionResult Edit(int id)
        {
            Order ord = _db.Orders.Find(id);

            ViewBag.Orders = ord;

            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, Order requestOrd)
        {
            Order ord = _db.Orders.Find(id);

            try
            {
                ord.Id_Order = requestOrd.Id_Order;

                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Edit", ord.Id_Order);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Order ord = _db.Orders.Find(id);

            _db.Orders.Remove(ord);

            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
