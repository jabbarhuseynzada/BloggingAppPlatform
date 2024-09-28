using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloggingAppPlatform.MVC.Areas.Admin.Controllers
{
    [Authorize(Policy = "AdminOrModerator")]
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
