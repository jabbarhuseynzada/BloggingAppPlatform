using Microsoft.AspNetCore.Mvc;

namespace BloggingAppPlatform.MVC.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
    }
}
