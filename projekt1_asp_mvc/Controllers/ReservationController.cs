using domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using objekty;

namespace projekt1_asp_mvc.Controllers
{
	public class ReservationController : Controller
	{
		private logic_domain1 _domain;

		public ReservationController(logic_domain1 log_dom)
		{
			_domain = log_dom;
		}

		public ActionResult Index()
		{
            if (HttpContext.Session.GetString("user") == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var reservations = _domain.get_reservations_uid(HttpContext.Session.GetString("user"));
			ViewBag.reservations = reservations.OrderBy(x => x.DateID);
			//return View(reservations);
			return View();
		}

		public ActionResult Delete(string cour, DateTime dat)
		{
			reservation? res = _domain.get_reservation(cour, dat);
			if (res == null || res.DateID < DateTime.Now) { return RedirectToAction("Index"); }

			_domain.remove_reservation(res);

			return RedirectToAction("Index");
		}


	}
}
