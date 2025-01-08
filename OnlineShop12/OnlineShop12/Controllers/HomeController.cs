using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop12.Data;
using OnlineShop12.Models;

namespace OnlineShop12.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
       

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            SetAccessRights();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
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
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
