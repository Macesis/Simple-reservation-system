using domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using objekty;

namespace projekt1_asp_mvc.Controllers
{
	public class CourtController : Controller
	{
		private logic_domain1 _domain;

		public CourtController(logic_domain1 log_dom)
		{
			_domain = log_dom;
		}

		public IActionResult Index()
		{
			var courts = _domain.courts;
			ViewBag.courts = courts;
			return View();
		}

		public IActionResult Detail(string id, bool not_ok, string error_msg = "", DateTime? def_start = null, int def_hour = 0) {

            if(HttpContext.Session.GetString("user") == null)
			{
				return RedirectToAction("Login", "Auth");
			}

            var court = _domain.get_court(id);
			ViewBag.court = court;

			var reservations = _domain.get_reservations_cid(id);
			ViewBag.reservations = reservations.OrderBy(r => r.DateID).Where(r => r.DateID > DateTime.Now);

			ViewBag.ReserveError = "";

			if (not_ok)
			{
				ViewBag.ReserveError = error_msg;
				ViewBag.resStart = def_start;
				ViewBag.hours = def_hour;
			}
			

			return View();
		}

		[HttpPost]
		public IActionResult Reserve(string court_id, DateTime res_start, string hours)
		{
            if (HttpContext.Session.GetString("user") == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            try
			{
				int par_hour = int.Parse(hours);
				res_start = res_start.AddHours(par_hour);

				court court = _domain.get_court(court_id);


				//Console.WriteLine("Court: " + court_id);
				//Console.WriteLine("Start: " +  res_start);
				//Console.WriteLine("Hour: " +  hours);
				//reserve(string courtid, DateTime date, int price, string userid)
				_domain.reserve(court_id, res_start, court.Price, HttpContext.Session.GetString("user"));

				return RedirectToAction("Detail", new { id = court.CourtID });
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);

				// Redirect back to the Detail action with the invalid data
				return RedirectToAction("Detail", new { id = court_id, not_ok = true, error_msg = ex.Message, def_start = res_start, def_hour = hours });
			}

		}
	}
}
