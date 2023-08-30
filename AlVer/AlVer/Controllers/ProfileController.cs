using AlVer.Classes;
using AlVer.Models;
using AlVer.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace AlVer.Controllers
{
	[Authorize(Roles ="Customer")]
	public class ProfileController : Controller
	{
		private readonly AppDBContext _context;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		public ProfileController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDBContext context)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_roleManager = roleManager;
			_context = context;
		}
		[HttpGet]
		public async Task<IActionResult> Favorites()
		{
			var user = await _userManager.GetUserAsync(User);

			var favorites = _context.Favorites.Include(x=> x.Product).Where(x => x.UserId == user.Id).ToList();

			return View(favorites);
		}

		[HttpGet]
		public async Task<IActionResult> Basket()
		{
			var user = await _userManager.GetUserAsync(User);

			var basket = _context.Baskets.Include(x=> x.Product).Where(x => x.UserId == user.Id).ToList();

			decimal basketTotal = 0;
			foreach (var item in basket)
			{
				basketTotal += item.Product.Price * item.Amount;
			}
			ViewBag.BasketTotal = basketTotal.ToString("F");
			return View(basket);
		}

		[HttpGet]
		public async Task<IActionResult> Checkout()
		{
			var user = await _userManager.GetUserAsync(User);

			var basket = _context.Baskets.Include(x=> x.Product).Where(x => x.UserId == user.Id).ToList();

			decimal basketTotal = 0;
			foreach (var item in basket)
			{
				basketTotal += item.Product.Price * item.Amount;
			}
			ViewBag.Email = user.Email; 
			ViewBag.Address = user.Address;
			ViewBag.BasketTotal = basketTotal.ToString("F");
			ViewBag.UserId = user.Id;
			ViewBag.Phone = user.PhoneNumber;
			ViewBag.UserName = user.UserName.Split('@')[0];
			return View(basket);
		}

		[HttpPost]
		public IActionResult Checkout(Bill bill)
		{
			if (bill.Price == 0)
			{
				ViewBag.Empty = "Sepetiniz boş olduğu için herhangi bir işlem gerçekleştiremedik.";
				return View();
			}

            bill.Date = DateTime.UtcNow;
			bill.Situation = "Sipariş Alındı";
			
            _context.Bills.Add(bill);
            _context.SaveChanges();

			var basket = _context.Baskets.Include(x=> x.Product).Where(x => x.UserId == bill.UserId).ToList();

            foreach (var item in basket)
			{
				Order order = new Order();
				order.Amount = item.Amount;
				order.UserId = item.UserId;
				order.ProductId = item.ProductId;
				order.BillId = bill.BillId;
				order.Price = item.Product.Price;

				_context.Orders.Add(order);
				var baskets = _context.Baskets.FirstOrDefault(x => x.BasketId == item.BasketId);
				_context.Baskets.Remove(baskets);
			}

			_context.SaveChanges();
			ViewBag.Success = "Siparişiniz başarıyla alındı. En kısa sürede sizlere ulaştıracağız.";
			return View();
		}

		[HttpGet]
		public IActionResult AddAmountBasket(int basketId,int which)
		{
			var basket = _context.Baskets.FirstOrDefault(x=> x.BasketId == basketId);
			if (which == 1)
			{
                basket.Amount -= 1;
            }
			else
			{
                basket.Amount += 1;
            }
			if (basket.Amount ==0)
			{
				_context.Baskets.Remove(basket);
				_context.SaveChanges();
                return RedirectToAction("Basket");
            }
			_context.Baskets.Update(basket);
			_context.SaveChanges();
			return RedirectToAction("Basket");
		}

		[HttpGet]
		public IActionResult AddAndDeleteFromFavorite(int id,string userId,int productId)
		{
			var remove = _context.Favorites.FirstOrDefault(x => x.FavoriteId == id);

			_context.Favorites.Remove(remove);

			var check = _context.Baskets.FirstOrDefault(x=> x.UserId == userId&& x.ProductId == productId);
			if (check == null)
			{
                Basket newBasket = new Basket();
                newBasket.UserId = userId;
                newBasket.Amount = 1;
                newBasket.ProductId = productId;
                _context.Baskets.Add(newBasket);
            }
			else
			{
				check.Amount += 1;
				_context.Baskets.Update(check);
			}
			
			_context.SaveChanges();
			return RedirectToAction("Favorites");
		}

        [HttpPost]
        public int DeleteFromBasket(int id)
        {
            var remove = _context.Baskets.FirstOrDefault(x => x.BasketId == id);
            try
            {
                _context.Baskets.Remove(remove);
                _context.SaveChanges();
                return 1;
            }
            catch (Exception)
            {

                return 0;
            }
        }

        [HttpPost]
        public int DeleteFromFavorites(int id)
        {
            var remove = _context.Favorites.FirstOrDefault(x => x.FavoriteId == id);
            try
            {
                _context.Favorites.Remove(remove);
                _context.SaveChanges();
                return 1;
            }
            catch (Exception)
            {

                return 0;
            }
        }
        
		public async Task<IActionResult> Orders ()
		{
			var user = await _userManager.GetUserAsync(User);
			var orders = _context.Bills.Where(x => x.UserId == user.Id).OrderByDescending(x=> x.BillId).ToList();
			return View(orders);
		}
		public IActionResult OrderDetails(int id)
		{
			var model = _context.Orders.Include(x=> x.Product).Include(x=> x.Bill).Where(x => x.BillId == id).ToList();

            var bill = _context.Bills.FirstOrDefault(x => x.BillId == id);
            ViewBag.TotalPrice = bill.Price;
            ViewBag.OrderSituation = bill.Situation;
            ViewBag.Name = bill.Name +" " + bill.Surname;
            ViewBag.Email = bill.Email;
            ViewBag.Address = bill.Address;
            ViewBag.PostCode = bill.PostCode;
            ViewBag.Phone = bill.Phone;

            return View(model);
        }

		[HttpGet]
		public IActionResult ChangePassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ChangePassword(string oldPassword,string newPassword,string passwordAgain)
		{
			if (newPassword.Length >= 6)
			{
                if (newPassword == passwordAgain)
                {
                    var user = await _userManager.GetUserAsync(User);
                    var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
                    if (result.Succeeded)
                    {
                        ViewBag.Success = "Şifreniz başarıyla değiştirildi.";
                        return RedirectToAction("ChangePassword");
                    }
                    else
                    {
                        ViewBag.Error = "Eski şifreniz yanlış veya bir hata oluştu.";
                        return RedirectToAction("ChangePassword");
                    }
                }
                else
                {
                    ViewBag.NotMatch = "Yazdığınız yeni şifre ve tekrarı birbiriyle uyuşmuyor.";
                    return RedirectToAction("ChangePassword");
                }
            }
			else
			{
                ViewBag.TooShort = "Yeni şifreniz en az 6 karakterden oluşmalı.";
                return RedirectToAction("ChangePassword");
            }
			
        }

		[HttpGet]
		public async Task<IActionResult> Informations()
		{
			var user = await _userManager.GetUserAsync(User);
			return View(user);
		}

		[HttpPost]
		public async Task<IActionResult> Informations(string address,string phone)
		{
			var user = await _userManager.GetUserAsync(User);
			user.Address = address;
			user.PhoneNumber = phone;

			_context.Update(user);
			_context.SaveChanges();
			return RedirectToAction("Informations");
		}

    }
}
