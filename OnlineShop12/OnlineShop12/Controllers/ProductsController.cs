using Microsoft.AspNetCore.Mvc;
using OnlineShop12.Models;
using OnlineShop12.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
namespace OnlineShop12.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ProductsController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            _db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        
        public IActionResult Index()
        {
            if(User.IsInRole("Admin"))
            {
                var products = _db.Products.Include("Category");
                      

                ViewBag.Products = products;

                return View();
            }
            else
            {
                var products = _db.Products.Include("Category")
                          .Where(prod => prod.isApproved);
                //var products = from prod in _db.Products
                //                 select prod;
                ViewBag.Products = products;

                return View();
            }
           
        }

        public IActionResult Aproba(int id)
        {
            if(User.IsInRole("Admin"))
            {
                Product product = _db.Products.Include("Category").Include("Reviews")
                             .Where(prod => prod.Id_Product == id)
                             .First();
                product.isApproved = true;
                ViewBag.Products = product;

                _db.SaveChanges();

                return RedirectToAction("Index") ;
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public IActionResult Show(int id)
        {
            if (User.IsInRole("Admin"))
            {
                ViewBag.EsteAdmin = 1;
                Product product = _db.Products.Include("Category").Include("Reviews")
                             .Where(prod => prod.Id_Product == id)
                             .First();
                Console.WriteLine(product.Id_Product);
                ViewBag.Products = product;

                return View(product);
            }
            else
            {
                ViewBag.EsteAdmin = 0;
                ViewBag.EsteColaborator = 0;
                if(User.IsInRole("Colaborator"))
                {
                    ViewBag.EsteColaborator = 1;
                }
                Product product = _db.Products.Include("Category").Include("Reviews")
                             .Where(prod => prod.Id_Product == id)
                             .First();
                Console.WriteLine(product.Id_Product);
                ViewBag.Products = product;

                return View(product);
            }
        }

        [HttpPost]
        public IActionResult Show([FromForm] Review review)
        {
            review.Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.Write(error.ErrorMessage);
                }
                _db.Reviews.Add(review);
                _db.SaveChanges();
                return Redirect("/Products/Show/" + review.Id_Product);
            }

            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.Write(error.ErrorMessage);
                }
                Product prod = _db.Products.Include("Category").Include("Reviews")
                              .Where(prod => prod.Id_Product == review.Id_Product)
                              .First();
                ViewBag.Products = prod;

                return View(prod);
            }
        }
        [Authorize(Roles ="Colaborator")]
        public IActionResult New()
        {
           
            Product product = new Product();
            product.Categ = GetAllCategories();
            ViewBag.Product = product;
            return View(product);
        }

        [HttpPost]
        public IActionResult New(Product product)
        {
            //Console.WriteLine($"Id_Category: {product.Id_Category}");
            product.Categ = GetAllCategories();
            //Console.Write(product.Categ);
            if (ModelState.IsValid)
            {
                _db.Products.Add(product);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.Write(error.ErrorMessage);
            }

           // ViewBag.Product = product;
            //else { ViewBag.Products=product;
            //       return View(product); }
            return View(product);
                     
                  
        }

        [Authorize(Roles ="Colaborator")]
        public IActionResult Edit(int id)
        {
            Product product = _db.Products.Include("Category")
                           .Where(prod => prod.Id_Product == id)
                           .First();
            product.Categ = GetAllCategories();

            //ViewBag.Products = prod;

            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(int id, Product requestProd)
        {
            Product prod = _db.Products.Find(id);
           // requestProd.Categ = GetAllCategories();
            if (ModelState.IsValid)
            {
                prod.Title = requestProd.Title;
                prod.Description = requestProd.Description;
                prod.Id_Category = requestProd.Id_Category;
                prod.isApproved = false;
                //TempData["message"] = "Articolul a fost modificat";
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                requestProd.Categ = GetAllCategories();

                return View(requestProd);
            }

            //try
            //{
            //    prod.Title = requestProd.Title;

            //    _db.SaveChanges();

            //    return RedirectToAction("Index");
            //}
            //catch (Exception)
            //{
            //    return RedirectToAction("Edit", prod.Id_Product);
            //}
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Product prod = _db.Products.Find(id);
            //Review review = _db.Reviews.Find(id);
            //Console.WriteLine(prod.Id_Product);
            _db.Products.Remove(prod);

            _db.SaveChanges();

            return RedirectToAction("Index");
            
        }
        
        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            // generam o lista de tipul SelectListItem fara elemente
            var selectList = new List<SelectListItem>();

            // extragem toate categoriile din baza de date
            var categories = from categ in _db.Categories
                             select categ;

            // iteram prin categorii
            foreach (var category in categories)
            {
                // adaugam in lista elementele necesare pentru dropdown
                // id-ul categoriei si denumirea acesteia
                selectList.Add(new SelectListItem
                {
                    Value = category.Id_Category.ToString(), //.ToString()
                    Text = category.Category_Name
                });
            }
            /* Sau se poate implementa astfel: 
             * 
            foreach (var category in categories)
            {
                var listItem = new SelectListItem();
                listItem.Value = category.Id.ToString();
                listItem.Text = category.CategoryName;

                selectList.Add(listItem);
             }
            */

            // returnam lista de categorii
            return selectList;
        }
        

    }
}
