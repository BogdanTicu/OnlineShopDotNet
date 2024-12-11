using Microsoft.AspNetCore.Mvc;
using OnlineShop12.Data;
using OnlineShop12.Models;

namespace OnlineShop12.Controllers
{
    public class RatingsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public RatingsController(ApplicationDbContext context)
        {
            _db = context;
        }

        [HttpPost]
        public IActionResult New(Rating rat)
        {
            rat.Date = DateTime.Now;

            if (ModelState.IsValid)
            {
                _db.Ratings.Add(rat);

                _db.SaveChanges();

                return Redirect("/Products/Show/" + rat.Id_Product);

            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.Write(error.ErrorMessage);
                }
                return Redirect("/Products/Show/" + rat.Id_Product);
            }
        }


            public IActionResult Edit(int id)
            {
                Rating rat = _db.Ratings.Find(id);


                return View(rat);
            }

            [HttpPost]
            public IActionResult Edit(int id, Rating requestRat)
            {
                Rating rat = _db.Ratings.Find(id);

                if (ModelState.IsValid)
                {

                    rat.Value = requestRat.Value;

                    _db.SaveChanges();

                    return Redirect("/Products/Show/" + rat.Id_Product);
                }
                else
                {
                    return View(requestRat);
                }
            
             }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Rating rat = _db.Ratings.Find(id);

            _db.Ratings.Remove(rat);

            _db.SaveChanges();

            return Redirect("/Products/Show/" + rat.Id_Product);
        }
    }
}
