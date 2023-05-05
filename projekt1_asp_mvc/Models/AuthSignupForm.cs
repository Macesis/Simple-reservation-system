using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace projekt1_asp_mvc.Models
{
    public class AuthSignupForm
    {
        [Display(Name = "Uživatelské jméno")]
        [Required(ErrorMessage = "Uživatelské jméno je povinné")]
        public string? Username { get; set; }

        [Display(Name = "Heslo")]
        [Required(ErrorMessage = "Heslo je povinné")]
        public string? Password { get; set; }

        [Display(Name = "Heslo znovu")]
        [Compare("Password", ErrorMessage ="Hesla se neshodují")]
        //[Required(ErrorMessage = "Heslo je povinné")]
        public string? ConfPassword { get; set; }

        [Display(Name = "Jméno")]
        [Required(ErrorMessage = "Jméno je povinné")]
        public string? FirstName { get; set; }

        [Display(Name = "Příjmení")]
        [Required(ErrorMessage = "Příjmení je povinné")]
        public string? LastName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email je povinný")]
        [EmailAddress(ErrorMessage = "Email není ve správném formátu")]
        public string? Email { get; set; }
    }
}
