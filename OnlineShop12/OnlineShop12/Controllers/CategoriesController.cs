using Microsoft.AspNetCore.Mvc;
using OnlineShop12.Models;
using OnlineShop12.Data;
using System;

namespace OnlineShop12.Controllers
{
    public class CategoriesController : Controller
    {

        private readonly ApplicationDbContext _db;

        public CategoriesController(ApplicationDbContext context)
        {
            _db = context;
        }
        public IActionResult Index()
        {
            var categories = from categ in _db.Categories
                             select categ;

            ViewBag.Categories = categories;

            return View();
        }

        public ActionResult Show(int id)
        {
            Category categ = _db.Categories.Find(id);

            ViewBag.Category = categ;

            return View();
        }

        public IActionResult New()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult New(Category categ)
        {
            Console.Write("Metoda New a fost apelată.");

            if (ModelState.IsValid)
            {
                Console.Write($"Adăugare categorie: {categ.Category_Name}");
                _db.Categories.Add(categ);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            Console.Write("Modelul este invalid.");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.Write(error.ErrorMessage);
            }

            return View(categ);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var categ = _db.Categories.Find(id);
            if (categ == null)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.Write(error.ErrorMessage);
                }
                return NotFound();
            }

            return View(categ); // Trimitem obiectul către View
        }

        [HttpPost]
        public ActionResult Edit(int id, Category requestCateg)
        {
            var categ = _db.Categories.Find(id);
            if (categ == null) return NotFound();

            if (ModelState.IsValid)
            {
                categ.Category_Name = requestCateg.Category_Name;
                categ.Description = requestCateg.Description;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(requestCateg);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var categ = _db.Categories.Find(id);
            if (categ == null) return NotFound();

            _db.Categories.Remove(categ);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
    

