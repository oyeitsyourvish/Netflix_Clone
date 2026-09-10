using Microsoft.AspNetCore.Mvc;

namespace NetflixClone.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
