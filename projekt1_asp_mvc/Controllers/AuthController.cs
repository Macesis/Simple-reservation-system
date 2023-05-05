using domain;
using Microsoft.AspNetCore.Mvc;
using objekty;
using projekt1_asp_mvc.Models;

namespace projekt1_asp_mvc.Controllers
{
	public class AuthController : Controller
	{
		private logic_domain1 _domain;

		public AuthController(logic_domain1 log_dom)
		{
			_domain = log_dom;
		}

		public IActionResult Index()
		{
			//return View();
			//check if user is logged in
			if (HttpContext.Session.Get("user") != null)
			{
				HttpContext.Session.Remove("user");
			}
			return RedirectToAction("Login");
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Login(AuthLoginForm loginForm)
		{
			if (ModelState.IsValid)
			{
				string username = loginForm.Username;
				string password = loginForm.Password;

				if(_domain.users.Any(u => u.UserID == username && u.Password == password))
				{
					HttpContext.Session.SetString("user", username);
					return RedirectToAction("Index", "Reservation");
				}
				ViewBag.Error = "Uživatelské jméno nebo heslo nejsou správné";
				return View();
			}
			return View();
		}

		public IActionResult Signup()
		{
			return View();
		}

		[HttpPost]
		public IActionResult SignUp(AuthSignupForm signupFrom)
		{
			if (ModelState.IsValid)
			{
				string username = signupFrom.Username;

				if(_domain.users.Any(u => u.UserID == username))
				{
					ViewBag.Error = "Uživatelské jméno již existuje";
					return View();
				}
				HttpContext.Session.SetString("user", username);
				_domain.add_user(new user(username, signupFrom.Password,signupFrom.FirstName,signupFrom.LastName,signupFrom.Email));
				return RedirectToAction("Index", "Reservation");
			}
			return View();
		}
	}
}
