using AlVer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AlVer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDBContext _context;

        public HomeController(ILogger<HomeController> logger, AppDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Categories = _context.Category.ToList();
            var products = _context.Products.OrderBy(x=>x.ProductId).Take(8).ToList();
            ViewBag.Last = _context.Products.OrderByDescending(x=> x.ProductId).Take(3).ToList();
            ViewBag.Last2 = _context.Products.OrderByDescending(x => x.ProductId).Skip(3).Take(3).ToList();
            ViewBag.Most = _context.Products.OrderBy(x => x.Stock).Take(3).ToList();
            ViewBag.Most2 = _context.Products.OrderBy(x => x.Stock).Skip(3).Take(3).ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult Shop(int page, int category, int sort, string search)
        {
            if (page == 0)
            {
                page = 1;
            }
            ViewBag.Categories = _context.Category.ToList();
            ViewBag.Last = _context.Products.OrderByDescending(x => x.ProductId).Take(3).ToList();
            ViewBag.Last2 = _context.Products.OrderByDescending(x => x.ProductId).Skip(3).Take(3).ToList();
            ViewBag.Sale = _context.Products.OrderBy(x => x.Price).Take(6).ToList();
            ViewBag.Page = page;


            var model = _context.Products.ToList();

            if (category != 0)
            {
                model =  model.Where(x => x.CategoryId == category).ToList(); 
            }
            if(sort != 0) 
            {
                if (sort == 1)
                {
                    model = model.OrderByDescending(x => x.Price).ToList();
                }
                else if (sort == 2) 
                {
                    model = model.OrderBy(x => x.Price).ToList();
                }
                else if(sort == 3) 
                {
                    model = model.OrderByDescending(x=> x.ProductId).ToList();
                }
            }
            if (search != null)
            {
                model = model.Where(x=> x.Name.ToLower().Contains(search.ToLower().Replace('ı', 'i'))).ToList();
            }

            var productCount = Convert.ToDouble(model.Count);
            double pageDouble = productCount / 15;
            double pageRounding = Math.Ceiling(pageDouble);
            int pageCount = Convert.ToInt32(pageRounding);
            ViewBag.PageCount = pageCount;
            ViewBag.Category = category;
            ViewBag.Sort = sort;
            ViewBag.Search = search;

            var products = model.Skip((page - 1) * 15).Take(15).ToList();
            ViewBag.Count = model.Count;
            return View(products);
        }

        [HttpPost]
        public IActionResult SearchTextDirector(string searchText)
        {
            return RedirectToAction("Shop", new { search = searchText });
        }

    }
}