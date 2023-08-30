using System.ComponentModel.DataAnnotations;

namespace AlVer.ViewModel
{
	public class LoginViewModel
	{
		public string username { get; set; }
		public string namesurname { get; set; }

		[StringLength(20,ErrorMessage = "Şifreniz en az 6 en fazla 20 karakterden oluşmalıdır.",MinimumLength =6)]
		public string password { get; set; }

		[Compare("password",ErrorMessage = "Şifre ve tekrarı birbiriyle uyuşmuyor.")]
		public string passwordagain { get; set; }
		
	}
}
