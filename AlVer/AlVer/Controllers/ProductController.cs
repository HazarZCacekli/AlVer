using AlVer.Models;
using AlVer.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AlVer.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<AppUser> _userManager;
        public ProductController(AppDBContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult ProductDetails(int id)
        {
            var products = _context.Products.Include(x=> x.Category).FirstOrDefault(x => x.ProductId == id);

            ViewBag.Similar = _context.Products.Where(x => x.CategoryId == products.CategoryId && x.ProductId != id).Take(4).ToList();

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> AddToBasket(int id,int which, int page,int category , int sort)
        {
            if (User.Identity.Name == null)
            {
                return RedirectToAction("Login", "Profile");
            }

            var user = await _userManager.GetUserAsync(User);

            var check = _context.Baskets.FirstOrDefault(x=> x.UserId == user.Id && x.ProductId == id);

            if (check == null) 
            {
                Basket basket = new Basket();
                basket.UserId = user.Id;
                basket.ProductId = id;
                basket.Amount = 1;

                _context.Baskets.Add(basket);
                _context.SaveChanges();
                if (which == 1)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Shop", "Home", new {page = page,category = category,sort = sort});
                }
            }
            else
            {
                check.Amount += 1;
                _context.Update(check);
                _context.SaveChanges();

                if (which == 1)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Shop", "Home", new { page = page, category = category, sort = sort });
                }
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> AddToBasketPost(int id,int amount)
        {
            if (User.Identity.Name == null)
            {
                return RedirectToAction("Login", "Profile");
            }
            var user = await _userManager.GetUserAsync(User);

            var check = _context.Baskets.FirstOrDefault(x => x.UserId == user.Id && x.ProductId == id);

            if (check == null)
            {
                Basket basket = new Basket();
                basket.UserId = user.Id;
                basket.ProductId = id;
                basket.Amount = amount;

                _context.Baskets.Add(basket);
                _context.SaveChanges();

                return RedirectToAction("ProductDetails", new {id = id });
            }
            else
            {
                check.Amount += amount;
                _context.Update(check);
                _context.SaveChanges();
                return RedirectToAction("ProductDetails", new { id = id });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToFavoritesPost(int id)
        {
            if (User.Identity.Name == null)
            {
                return RedirectToAction("Login", "Profile");
            }
            var user = await _userManager.GetUserAsync(User);
            var check = _context.Favorites.FirstOrDefault(x => x.UserId == user.Id && x.ProductId == id);

            if (check == null)
            {
                Favorite favorite = new Favorite();
                favorite.UserId = user.Id;
                favorite.ProductId = id;
                _context.Favorites.Add(favorite);
                _context.SaveChanges();
                return RedirectToAction("ProductDetails", new { id = id });
            }
            else
            {
                return RedirectToAction("ProductDetails", new { id = id });
            }

        }

        [HttpGet]
        public async Task<IActionResult> AddToFavorites(int id, int which,int page, int category, int sort)
        {
            if (User.Identity.Name == null)
            {
                return RedirectToAction("Login", "Profile");
            }

            var user = await _userManager.GetUserAsync(User);

            var check = _context.Favorites.FirstOrDefault(x => x.UserId == user.Id && x.ProductId == id);

            if (check == null)
            {
                Favorite favorite = new Favorite();
                favorite.ProductId = id;
                favorite.UserId = user.Id;

                _context.Favorites.Add(favorite);
                _context.SaveChanges();

                if (which == 1)
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (which == 2) 
                {
                    return RedirectToAction("ProductDetails", "Product", new {page = page, category = category, sort = sort });
                }
                else
                {
                    return RedirectToAction("Shop", "Home", new { page = page, category = category, sort = sort });
                }
            }
            else
            {
                if (which == 1)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Shop", "Home", new { page = page, category = category, sort = sort });
                }
            }
        }
    }
}
