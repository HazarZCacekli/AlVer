using System.ComponentModel.DataAnnotations;

namespace AlVer.ViewModel
{
    public class ResetPasswordViewModel
    {
        public string Password { get; set; }

		[Compare("Password", ErrorMessage = "Girdiğiniz yeni şifreyle tekrarı birbiriyle uyuşmuyor.")]
		public string ConfirmPassword { get; set; }

        public string Email { get; set; }
        public string Token { get; set; }
    }
}
