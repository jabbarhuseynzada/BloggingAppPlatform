using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloggingAppPlatform.MVC.Areas.Admin.Controllers
{
    public class CommentsController : Controller
    {
        [Authorize(Policy = "AdminOrModerator")]
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
