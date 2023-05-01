using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Theme.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
