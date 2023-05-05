using System.ComponentModel.DataAnnotations;

namespace projekt1_asp_mvc.Models
{
	public class AuthLoginForm
	{
		[Display (Name = "Uživatelské jméno")]
		[Required(ErrorMessage = "Uživatelské jméno je povinné")]
		public string? Username { get; set; }

		[Display(Name = "Heslo")]
		[Required(ErrorMessage = "Heslo je povinné")]
		public string? Password { get; set; }
	}
}
