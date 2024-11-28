using BloggingAppPlatform.MVC.Areas.Admin.ViewModels;
using Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloggingAppPlatform.MVC.Areas.Admin.Controllers
{
    [Authorize(Policy = "AdminOrModerator")]
    [Area("Admin")]
    public class ReportsController : Controller
    {
        private readonly IReportService _reportService;
        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ReportVM vm = new()
            {
                Reports = _reportService.GetAllReports().Data,
            };
            return View("Index", vm);

        }
        [HttpPost]
        public IActionResult DeleteReport(int Id)
        {
            var token = Request.Cookies["auth_token"];
            var result =_reportService.DeleteReport(Id);
            if (result.Success)
            {
                TempData["Message"] = "Post deleted successfully.";
            }
            else
            {
                TempData["Error"] = result.Message;
            }

            return RedirectToAction("Index", "Report", "Admin");
        }
    }
}
