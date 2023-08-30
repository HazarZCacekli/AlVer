using AlVer.Classes;
using AlVer.Models;
using AlVer.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlVer.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public LoginController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // GİRİŞ-ÇIKIŞ İŞLEMLERİ - ŞİFRE UNUTTUM - KAYIT VSVS.

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel user, bool rememberMe)
        {
            var result = await _signInManager.PasswordSignInAsync(user.username, user.password, rememberMe, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Email veya şifre yanlış.";
            }
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(LoginViewModel register)
        {
			if (!ModelState.IsValid)
			{
				return View();
			}
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
                await _userManager.AddToRoleAsync(user, "Customer");
                TempData["Success"] = "Başarıyla kayıt oldunuz. Lütfen giriş yapınız.";
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Error = "Bu E-Mail'e kayıtlı bir hesap zaten bulunmaktadır.";
                return View();
            }
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                TempData["ResetInfo"] = "Şifre sıfırlama linki E-Mail'inize gönderilmiştir.";
                return View();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var link = Url.Action("ResetPassword", "Login", new { token, email = user.Email, }, Request.Scheme);

            string mail = "Şifrenizi sıfırlamak için tıklayın : " + link + " \n Eğer bu işlemi siz yapmadıysanız görmezden gelebilirsiniz. Birisi yanlışlıkla sizin e-mailinizi girmiş olabilir.";

            EmailHelper emailHelper = new EmailHelper();
            bool emailSend = emailHelper.SendEmail(user.Email, mail, "AlVer Şifre Sıfırlama");

            if (emailSend)
            {
                TempData["ResetInfo"] = "Şifre sıfırlama linki E-Mail'inize gönderilmiştir.";
                return RedirectToAction("ForgotPassword");
            }
            else
            {
                TempData["ResetError"] = "Bilinmeyen bir hata sebebiyle E-Mail gönderilemedi. Lütfen site yetkilileriyle iletişime geçin";
            }
            return View();

        }


        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            ResetPasswordViewModel model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPassword);
            }

            var user = await _userManager.FindByEmailAsync(resetPassword.Email);

            if (user == null)
            {
                return RedirectToAction("ForgotPassword");
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);

            if (result.Succeeded)
            {
                TempData["Info"] = "Şifreniz başarılı bir şekilde değiştirildi.Lütfen giriş yapınız";
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Error = "Bu bağlantının süresi dolmuş veya bir hata mevcut.";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
