using System.Web.Mvc;

namespace AppHarbor.SQLServerCEUnitTesting.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return Content("Hello world");
		}
	}
}
