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
                SetAccessRights();
                return AfisarePaginata(products);
            }
            else
            {
                var products = _db.Products.Include("Category")
                          .Where(prod => prod.isApproved && !prod.isDeleted);

                SetAccessRights();
                return AfisarePaginata(products);
            }
        }
        public IActionResult AfisarePaginata(IQueryable<Product>? products)
        {
            var search = "";
            var sortBy = HttpContext.Request.Query["sortBy"];
            if (Convert.ToString(HttpContext.Request.Query["search"]) != null)
            {
                search = Convert.ToString(HttpContext.Request.Query["search"]).Trim(); // eliminam spatiile libere 

                // Cautare in produs (Titlu si Descriere)

                List<int> productIds = _db.Products.Where
                                        (
                                         at => at.Title.Contains(search)
                                         || at.Description.Contains(search)
                                        ).Select(a => a.Id_Product).ToList();

                // Cautare in reviews
                List<int> productIdsOfReviewsWithSearchString = _db.Reviews
                                        .Where
                                        (
                                         c => c.Content.Contains(search)
                                        ).Select(c => (int)c.Id_Product).ToList();

                // Se formeaza o singura lista formata din toate id-urile selectate anterior
                List<int> mergedIds = productIds.Union(productIdsOfReviewsWithSearchString).ToList();


                // Lista articolelor care contin cuvantul cautat
                // fie in produs -> Title si Content
                // fie in review -> Content
                products = _db.Products.Where(prod => mergedIds.Contains(prod.Id_Product))
                                      .Include("Category")
                                      .Include("User");

                if (!string.IsNullOrEmpty(sortBy))
                {
                    if(sortBy == "price_asc")
                    {
                        products = products.OrderBy(p => p.Price);
                    }
                    else if(sortBy == "price_desc")
                    {
                        products = products.OrderByDescending(p => p.Price);
                    }
                    else if(sortBy == "score_asc")
                    {
                        products = products.OrderBy(p => p.Score);
                    }
                    else if(sortBy == "score_desc")
                    {
                        products = products.OrderByDescending(p => p.Score);
                    }
                }
            }

            ViewBag.SearchString = search;
            // AFISARE PAGINATA

            // Alegem sa afisam 3 articole pe pagina
            int _perPage = 3;

            // Fiind un numar variabil de articole, verificam de fiecare data utilizand 
            // metoda Count()

            int totalItems = products.Count();

            // Se preia pagina curenta din View-ul asociat
            // Numarul paginii este valoarea parametrului page din ruta
            // /Articles/Index?page=valoare

            var currentPage = Convert.ToInt32(HttpContext.Request.Query["page"]);

            // Pentru prima pagina offsetul o sa fie zero
            // Pentru pagina 2 o sa fie 3 
            // Asadar offsetul este egal cu numarul de articole care au fost deja afisate pe paginile anterioare
            var offset = 0;

            // Se calculeaza offsetul in functie de numarul paginii la care suntem
            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * _perPage;
            }

            // Se preiau articolele corespunzatoare pentru fiecare pagina la care ne aflam 
            // in functie de offset
            var paginatedProducts = products.Skip(offset).Take(_perPage);


            // Preluam numarul ultimei pagini
            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)_perPage);

            // Trimitem articolele cu ajutorul unui ViewBag catre View-ul corespunzator
            ViewBag.Products = paginatedProducts;
            if (search != "")
            {
                ViewBag.PaginationBaseUrl = "/Products/Index/?search=" + search + "&page";
            }
            else
            {
                ViewBag.PaginationBaseUrl = "/Products/Index/?page";
            }
            ViewBag.SortBy = sortBy;
            SetAccessRights();
            return View("Index", "Products"); 
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
                
                //Product product = _db.Products.Include("Category").Include("Reviews")
                Product product = _db.Products.Include("Category").Include("Reviews").Include("Ratings")
                             .Where(prod => prod.Id_Product == id)
                              .FirstOrDefault(prod => prod.Id_Product == id);
                if (product != null && product.Ratings.Count()!=0)
                {
                    // Calculăm media ratingurilor
                    double averageRating = product.Ratings.Average(r => r.Value);
                    product.Score = Math.Round(averageRating, 1); // Opțional, rotunjire la o zecimală

                    // Poți adăuga media calculată în ViewBag sau ca o proprietate a modelului
                    ViewBag.AverageRating = product.Score;
                }
                else if(product.Ratings.Count()==0)
                {
                    product.Score = 0;
                    ViewBag.AverageRating = 0;
                }
                _db.SaveChanges();
                Console.WriteLine(product.Id_Product);
                ViewBag.Products = product;

                SetAccessRights();
                return View(product);
            }
            else
            {
                
                ViewBag.EsteColaborator = 0;
                if(User.IsInRole("Colaborator"))
                {
                    ViewBag.EsteColaborator = 1;
                }
                Product product = _db.Products.Include("Category").Include("Reviews").Include("Ratings")
                             .Where(prod => prod.Id_Product == id)
                             .FirstOrDefault(prod => prod.Id_Product == id); ;
                //Console.WriteLine(product.Id_Product);
                if (product != null && product.Ratings.Count()!=0)
                {
                    // Calculăm media ratingurilor
                    double averageRating = product.Ratings.Average(r => r.Value);
                    product.Score = Math.Round(averageRating, 1); // Opțional, rotunjire la o zecimală

                    // Poți adăuga media calculată în ViewBag sau ca o proprietate a modelului
                    ViewBag.AverageRating = product.Score;
                }
                else if (product.Ratings.Count() == 0)
                {
                    product.Score = 0;
                    ViewBag.AverageRating = 0;
                }
                Console.WriteLine(product.Id_Product);
                _db.SaveChanges();
                ViewBag.Products = product;
                SetAccessRights();
                return View(product);
            }

        }

        [HttpPost]
        public IActionResult Show([FromForm] Review review)
        {
            review.Date = DateTime.Now;
            review.Id_User = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.Write(error.ErrorMessage);
                }
                _db.Reviews.Add(review);
                _db.SaveChanges();
                SetAccessRights();
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
                SetAccessRights();
                return View(prod);
            }
        }


        [HttpPost]
        public IActionResult Show2([FromForm] Rating rating)
        {
            rating.Date = DateTime.Now;
            rating.Id_User = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                _db.Ratings.Add(rating);
                _db.SaveChanges();
                SetAccessRights();
                return Redirect("/Products/Show/" + rating.Id_Product);
            }

            else
            {
                Product prod = _db.Products.Include("Category").Include("Ratings")
                             .Where(prod => prod.Id_Product == rating.Id_Product)
                             .First();

                ViewBag.Products = prod;
                SetAccessRights();
                return View(prod);
            }
        }

        [Authorize(Roles ="Colaborator")]
        public IActionResult New()
        {
           
            Product product = new Product();
            product.Categ = GetAllCategories();
            ViewBag.Product = product;
            SetAccessRights();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> New(Product product, IFormFile? imageFile)
        {
            product.Categ = GetAllCategories();
            
            //Console.Write(product.Categ);
            if (ModelState.IsValid)
            {
               // product.isDeleted = false;
               // product.isApproved = false;
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Salvarea fișierului
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Creează folderul dacă nu există
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    // Setează calea imaginii
                    product.ImagePath = $"/images/products/{uniqueFileName}";
                }
                SetAccessRights();
                _db.Products.Add(product);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.Write(error.ErrorMessage);
            }
            SetAccessRights();
            return View(product);
        }


        [Authorize(Roles ="Colaborator,Admin")]
        public IActionResult Edit(int id)
        {
            Product product = _db.Products.Include("Category")
                           .Where(prod => prod.Id_Product == id)
                           .First();
            product.Categ = GetAllCategories();

            //ViewBag.Products = prod;
            SetAccessRights();
            return View(product);
        }


            [HttpPost]
            public async Task<IActionResult> Edit(int id, Product requestProd, IFormFile? imageFile)
            {
            SetAccessRights();
                Product prod = _db.Products.Find(id);
                requestProd.Categ = GetAllCategories();

                if (ModelState.IsValid)
                {
                    prod.Title = requestProd.Title;
                    prod.Description = requestProd.Description;
                    prod.Id_Category = requestProd.Id_Category;
                    prod.Stock = requestProd.Stock;
                    prod.Price = requestProd.Price;
                    prod.isApproved = false;
                    if(User.IsInRole("Admin"))
                    {
                        prod.isApproved = true;
                    }
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Șterge imaginea veche
                        if (!string.IsNullOrEmpty(prod.ImagePath))
                        {
                            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", prod.ImagePath.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Salvează imaginea nouă
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }

                        prod.ImagePath = $"/images/products/{uniqueFileName}";
                    }

                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(requestProd);
            }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            Product prod = _db.Products.Find(id);
            //Review review = _db.Reviews.Find(id);
            //Console.WriteLine(prod.Id_Product);
            if(User.IsInRole("Admin"))
            {
                if (prod != null)
                {
                    // Șterge imaginea asociată
                    if (!string.IsNullOrEmpty(prod.ImagePath))
                    {
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", prod.ImagePath.TrimStart('/'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    _db.Products.Remove(prod);
                    _db.SaveChanges();
                }
            }
            else if(User.IsInRole("Colaborator"))
            {
                prod.isDeleted = true;
                _db.SaveChanges();
            }
           

            return RedirectToAction("Index");
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
            return selectList;
        }
        

    }
}
