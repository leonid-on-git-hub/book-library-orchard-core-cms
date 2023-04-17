using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Main.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return View();
            }
            return View("WelcomePage");
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}
