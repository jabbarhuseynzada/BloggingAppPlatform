using Microsoft.AspNetCore.Mvc;

namespace BloggingAppPlatform.MVC.Areas.AdminDashboard.Controllers
{
    [Area("AdminDashboard")]
    public class CommentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
