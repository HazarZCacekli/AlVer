using AlVer.Models;
using AlVer.Classes;
using Microsoft.AspNetCore.Mvc;

namespace AlVer.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDBContext _context;

        public ContactController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Message message)
        {
            message.Tarih = DateTime.UtcNow;
            _context.Messages.Add(message);
            _context.SaveChanges();
            TempData["Success"] = "Mesajınız başarıyla gönderildi. En kısa sürede sizlere dönüş yapacağız.";
            return View();
        }

        public IActionResult AddToNewsletter(Newsletter newsletter)
        {
            var emailCheck = _context.newsletters.FirstOrDefault(x => x.Email == newsletter.Email);
            if (emailCheck == null)
            {
                EmailHelper emailHelper = new EmailHelper();
                string subject = "AlVer E-Bülten Kayıt";
                string body = "AlVer E-Bültenimize kayıt olduğunuz için teşekkür ederiz. Gelecekte olacak haberlerden ve kampanyalardan sizi en kısa sürede haberdar edeceğiz.";
                emailHelper.SendEmail(newsletter.Email, body, subject);
                newsletter.Tarih = DateTime.UtcNow;
                _context.newsletters.Add(newsletter);
                _context.SaveChanges();
                TempData["Success"] = "Başarıyla kaydınız yapıldı.";
            }
            else
            {
                TempData["Error"] = "Email zaten kayıtlı.";
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
