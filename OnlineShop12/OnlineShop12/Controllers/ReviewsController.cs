using Microsoft.AspNetCore.Mvc;
using OnlineShop12.Models;
using OnlineShop12.Data;
using Azure.Core;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Identity;

namespace OnlineShop12.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ReviewsController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            _db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
       

        [HttpPost]
        public IActionResult New(Review rev)
        {
            rev.Date= DateTime.Now;

            if(ModelState.IsValid)
            {
                _db.Reviews.Add(rev);

                _db.SaveChanges();

                return Redirect("/Products/Show/"+ rev.Id_Product);

            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.Write(error.ErrorMessage);
                }
                return Redirect("/Products/Show/" + rev.Id_Product);
            }


        }

        public IActionResult Edit(int id)
        {
            Review rev = _db.Reviews.Find(id);

           // ViewBag.Review = rev;

            return View(rev);
        }

        [HttpPost]
        public IActionResult Edit(int id, Review requestRev)
        {
            Review rev = _db.Reviews.Find(id);
           // if (rev == null) return NotFound();

            if (ModelState.IsValid)
            {

                rev.Content = requestRev.Content;

                _db.SaveChanges();

                return Redirect("/Products/Show/" + rev.Id_Product);
            }
            else
            {
                return View(requestRev);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Review rev = _db.Reviews.Find(id);

            _db.Reviews.Remove(rev);

            _db.SaveChanges();

            return Redirect("/Products/Show/" + rev.Id_Product);
        }
    }
}
