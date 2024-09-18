using Microsoft.AspNetCore.Mvc;

namespace BloggingAppPlatform.MVC.Areas.AdminDashboard.Controllers
{
    public class DashboardController : Controller
    {
        [Area("AdminDashboard")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
