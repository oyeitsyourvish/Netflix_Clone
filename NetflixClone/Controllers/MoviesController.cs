using Microsoft.AspNetCore.Mvc;

namespace NetflixClone.Controllers
{
    public class MoviesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
