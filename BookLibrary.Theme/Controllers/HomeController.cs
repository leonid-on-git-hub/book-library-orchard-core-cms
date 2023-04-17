using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
