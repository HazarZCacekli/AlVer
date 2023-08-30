using AlVer.Classes;
using AlVer.Models;
using AlVer.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace AlVer.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFileProvider _fileProvider;
        public AdminController(AppDBContext context, UserManager<AppUser> userManager, IFileProvider fileProvider)
        {
            _context = context;
            _userManager = userManager;
            _fileProvider = fileProvider;
        }

        public IActionResult Index()
        {
            var TodaySale = _context.Bills.Where(x => x.Date.Day == DateTime.UtcNow.Day && x.Date.Month == DateTime.UtcNow.Month && x.Date.Year == DateTime.UtcNow.Year).ToList();
            decimal todaySale = 0;
            foreach (var item in TodaySale)
            {
                todaySale += item.Price;
            }
            ViewBag.TodaySale = todaySale;

            var AllSale = _context.Bills.ToList();
            decimal allSale = 0;
            foreach (var item in AllSale)
            {
                allSale += item.Price;
            }
            ViewBag.AllSale = allSale;

            var AllUsers = _context.Users.ToList();
            ViewBag.AllUsers = AllUsers.Count;

            var TodayUsers = _context.Users.Where(x=> x.Tarih.Day == DateTime.UtcNow.Day && x.Tarih.Month == DateTime.UtcNow.Month && x.Tarih.Year == DateTime.UtcNow.Year).ToList();
            ViewBag.TodayUsers = TodayUsers.Count;

            ViewBag.Messages = _context.Messages.OrderByDescending(x=> x.MessageId).Take(4).ToList();

            ViewBag.RecentSales = _context.Bills.OrderByDescending(x => x.UserId).Take(5).ToList();

            ViewBag.Tasks = _context.ToDoTasks.OrderByDescending(x => x.ToDoTaskId).Take(5).ToList();

            return View();
        }

        [HttpGet]
        public IActionResult Tasks()
        {
            ViewBag.Messages = _context.Messages.OrderByDescending(x => x.MessageId).Take(4).ToList();
            var model = _context.ToDoTasks.ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult AddTask(string taskBody, int which)
        {
            ToDoTask task = new ToDoTask();
            task.TaskBody = taskBody;
            task.Date = DateTime.UtcNow;
            _context.ToDoTasks.Add(task);
            _context.SaveChanges();
            return which==1? RedirectToAction("Index", "Admin") : RedirectToAction("Tasks", "Admin");
        }
        [HttpGet]
        public IActionResult DeleteTask(int id,int which)
        {
            var task = _context.ToDoTasks.FirstOrDefault(x => x.ToDoTaskId == id);
            _context.Remove(task);
            _context.SaveChanges();
            return which == 2 ? RedirectToAction("Tasks", "Admin") : RedirectToAction("Index", "Admin");
            
        }
        public IActionResult Messages() 
        {
            ViewBag.Messages = _context.Messages.OrderByDescending(x => x.MessageId).Take(3).ToList();
            var messages = _context.Messages.ToList();
            return View(messages);
        }

        [HttpGet]
        public IActionResult MessageDetails(int id)
        {
            ViewBag.Messages = _context.Messages.OrderByDescending(x => x.MessageId).Take(3).ToList();
            var message = _context.Messages.FirstOrDefault(x=> x.MessageId== id);
            return View(message);
        }

        [HttpGet]
        public IActionResult DeleteMessage (int id)
        {
            var mesaj = _context.Messages.FirstOrDefault(x => x.MessageId == id);
            try
            {
                _context.Messages.Remove(mesaj);
                _context.SaveChanges();
                return RedirectToAction("Messages");
            }
            catch (Exception e)
            {
                return RedirectToAction("Messages");
            }
        }

        [HttpGet]
        public IActionResult RegisterAdmin()
        {
            ViewBag.Messages = _context.Messages.OrderByDescending(x => x.MessageId).Take(3).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAdmin (LoginViewModel register)
        {
            ViewBag.Messages = _context.Messages.OrderByDescending(x => x.MessageId).Take(3).ToList();

            if (ModelState.IsValid)
            {
                AppUser user = new AppUser()
                {
                    Email = register.username,
                    UserName = register.username,
                    AdSoyad = register.namesurname,
                    Tarih = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, register.password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                    ViewBag.Success = "Kayıt Başarılı.";
                    return View();
                }
                else
                {
                    ViewBag.Error = "Bu E-Mail'e kayıtlı bir hesap zaten bulunmaktadır.";
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Sales(string searchText)
        {
            ViewBag.Messages = _context.Messages.OrderByDescending(x => x.MessageId).Take(3).ToList();

            if (searchText!= null)
            {
                var salesSearched = _context.Bills.Where(x => x.BillId.ToString() == searchText || x.UserName == searchText).ToList();
                return View(salesSearched);
            }

            var sales = _context.Bills.ToList();
            return View(sales);
        }

        [HttpPost]
        public IActionResult SearchTextDirector(string searchText,int which)
        {
            return which==1? RedirectToAction("Sales",new {searchText= searchText}) : RedirectToAction("Products", new { searchText = searchText });
        }

        [HttpGet]
        public IActionResult SaleDetails(int id) 
        {
            ViewBag.Messages = _context.Messages.OrderByDescending(x => x.MessageId).Take(3).ToList();

            var model = _context.Orders.Include(x=> x.Product).Include(x=> x.Bill).Where(x => x.BillId == id).ToList();

            ViewBag.Status = _context.Bills.FirstOrDefault(x => x.BillId == id).Situation;
            ViewBag.BillId = id;
            return View(model);
        }

        [HttpGet]
        public IActionResult OrderStatus (string status,int id)
        {
            var order = _context.Bills.FirstOrDefault(x=> x.BillId==id);
            order.Situation = status;
            _context.Update(order);
            _context.SaveChanges();
            return RedirectToAction("Sales");
        }

        [HttpGet]
        public IActionResult CancelSale(int id) 
        {
            var sale = _context.Bills.FirstOrDefault(x=> x.BillId== id);
            try
            {
                sale.Situation = "İptal Edildi.";
                _context.SaveChanges();
                return RedirectToAction("Sales");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Bir hata oluştu lütfen daha sonra tekrar deneyin.";
                return RedirectToAction("Sales");
            }
        }

        [HttpGet]
        public IActionResult Products(string searchText)
        {
            ViewBag.Messages = _context.Messages.OrderByDescending(x => x.MessageId).Take(3).ToList();
            if (searchText != null)
            {
                var productSearch = _context.Products.Where(x=> x.Name.ToLower().Contains(searchText.ToLower().Replace('ı','i')) || x.ProductId.ToString() == searchText).ToList();
                return View(productSearch);
            }

            var model = _context.Products.OrderBy(x=> x.Name).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult ProductDetails(int id)
        {
            ViewBag.Messages = _context.Messages.OrderByDescending(x => x.MessageId).Take(3).ToList();
            ViewBag.Category = _context.Category.ToList();

            var products = _context.Products.Include(x=> x.Category).FirstOrDefault(x => x.ProductId == id);

            return View(products);
        }

        [HttpPost]
        public IActionResult UpdateProduct(Product product, IFormFile photo, IFormFile photo2, IFormFile photo3, IFormFile photo4, IFormFile photo5)
        {
            if (photo != null) product.Image = AddPhoto(photo);
            if (photo2 != null) product.Image2 = AddPhoto(photo2);
            if (photo3 != null) product.Image3 = AddPhoto(photo3);
            if (photo4 != null) product.Image4 = AddPhoto(photo4);
            if (photo5 != null) product.Image5 = AddPhoto(photo5);
            _context.Products.Update(product);
            _context.SaveChanges();
            return RedirectToAction("Products");
        }

        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(x=> x.ProductId == id);
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Products", "Admin");
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            ViewBag.Messages = _context.Messages.OrderByDescending(x => x.MessageId).Take(3).ToList();
            ViewBag.Category = _context.Category.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(IFormFile photo, IFormFile photo2, IFormFile photo3, IFormFile photo4, IFormFile photo5, Product product)
        {
            if (photo != null) product.Image = AddPhoto(photo);
            if (photo2 != null) product.Image2 = AddPhoto(photo);
            if (photo3 != null) product.Image3 = AddPhoto(photo);
            if (photo4 != null) product.Image4 = AddPhoto(photo);
            if (photo5 != null) product.Image5 = AddPhoto(photo);

            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction("AddProduct");
        }

        [HttpGet]
        public IActionResult SendNewsletter()
        {
            ViewBag.Messages = _context.Messages.OrderByDescending(x => x.MessageId).Take(3).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult SendNewsletter(string subject, string body)
        {
            ViewBag.Messages = _context.Messages.OrderByDescending(x => x.MessageId).Take(3).ToList();
            var emails = _context.newsletters.ToList();
            foreach (Newsletter item in emails)
            {
                EmailHelper helper = new EmailHelper();
                helper.SendEmail(item.Email, body, subject);
            }
            return RedirectToAction("SendNewsletter");
        }

        [HttpGet]
        public IActionResult Users()
        {
            ViewBag.Messages = _context.Messages.OrderByDescending(x => x.MessageId).Take(3).ToList();
            var users = _context.Users.ToList();
            return View(users);
        }

        [HttpGet]
        public IActionResult UserDetails(string id)
        {
            ViewBag.Messages = _context.Messages.OrderByDescending(x => x.MessageId).Take(3).ToList();
            var user = _context.Users.FirstOrDefault(x=> x.Id == id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(AppUser user)
        {
            var updateuser = await _userManager.FindByIdAsync(user.Id);
            updateuser.AdSoyad = user.AdSoyad;
            updateuser.Email = user.Email;
            updateuser.Address = user.Address;
            updateuser.PhoneNumber = user.PhoneNumber;

            await _userManager.UpdateAsync(updateuser);
            _context.SaveChanges();
            return RedirectToAction("Users");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
            _context.SaveChanges();
            return RedirectToAction("Users");
        }

        public string AddPhoto (IFormFile photo)
        {
                var root = _fileProvider.GetDirectoryContents("wwwroot");
                var img = root.First(x => x.Name == "product");
                string randomImageName = Guid.NewGuid() + Path.GetExtension(photo.FileName);
                var path = Path.Combine(img.PhysicalPath, randomImageName);
                using var stream = new FileStream(path, FileMode.Create);
                photo.CopyTo(stream);

                return randomImageName;
        }
    }
}
