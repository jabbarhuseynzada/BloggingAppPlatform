using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloggingAppPlatform.MVC.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        [Authorize(Policy = "AdminOrModerator")]
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
