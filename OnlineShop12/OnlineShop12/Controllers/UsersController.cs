using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop12.Data;
using OnlineShop12.Models;

namespace OnlineShop12.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UsersController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            _db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [Authorize(Roles ="Admin")]
        public IActionResult Index()
        {
            var users = from user in _db.Users
                        orderby user.UserName
                        select user;

            ViewBag.UsersList = users;
            SetAccessRights();
            return View();
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Show(string id)
        {
            ApplicationUser user = _db.Users.Find(id);
            var roles = await _userManager.GetRolesAsync(user);

            ViewBag.Roles = roles;

            ViewBag.UserCurent = await _userManager.GetUserAsync(User);

            return View(user);
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(string id)
        {
            ApplicationUser user = _db.Users.Find(id);

            ViewBag.AllRoles = GetAllRoles();

            var roleNames = await _userManager.GetRolesAsync(user); // Lista de nume de roluri

            // Cautam ID-ul rolului in baza de date
            ViewBag.UserRole = _roleManager.Roles
                                           .Where(r => roleNames.Contains(r.Name))
                                           .Select(r => r.Id)
                                           .First(); // Selectam 1 singur rol

            return View(user);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(string id, ApplicationUser newData, [FromForm] string newRole)
        {
            ApplicationUser user = _db.Users.Find(id);

            user.AllRoles = GetAllRoles();


            if (ModelState.IsValid)
            {
                user.UserName = newData.UserName;
                user.Email = newData.Email;
                user.PhoneNumber = newData.PhoneNumber;


                // Cautam toate rolurile din baza de date
                var roles = _db.Roles.ToList();

                foreach (var role in roles)
                {
                    // Scoatem userul din rolurile anterioare
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                // Adaugam noul rol selectat
                var roleName = await _roleManager.FindByIdAsync(newRole);
                await _userManager.AddToRoleAsync(user, roleName.ToString());

                _db.SaveChanges();

            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(string id)
        {
            var user = _db.Users
                         .Include("Products")
                         .Include("Reviews")
                         .Include("Ratings")
                         .Where(u => u.Id == id)
                         .First();

            if (user.Reviews.Count > 0)
            {
                foreach (var comment in user.Reviews)
                {
                    _db.Reviews.Remove(comment);
                }
            }

            // Delete user bookmarks
            if (user.Ratings.Count > 0)
            {
                foreach (var bookmark in user.Ratings)
                {
                    _db.Ratings.Remove(bookmark);
                }
            }

           

            _db.Users.Remove(user);

            _db.SaveChanges();

            return RedirectToAction("Index");
        }


        [NonAction]
        public IEnumerable<SelectListItem> GetAllRoles()
        {
            var selectList = new List<SelectListItem>();

            var roles = from role in _db.Roles
                        select role;

            foreach (var role in roles)
            {
                selectList.Add(new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name.ToString()
                });
            }
            return selectList;
        }
        private void SetAccessRights()
        {
            ViewBag.AfisareButoane = false;

            if (User.IsInRole("Colaborator"))
            {
                ViewBag.AfisareButoane = true;
            }

            ViewBag.UserCurent = _userManager.GetUserId(User);

            ViewBag.EsteAdmin = User.IsInRole("Admin");
            ViewBag.EsteColaborator = User.IsInRole("Colaborator");
        }
    }
}
