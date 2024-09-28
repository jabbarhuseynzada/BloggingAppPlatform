using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BloggingAppPlatform.MVC.Models;

namespace BloggingAppPlatform.MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    //public IActionResult Error()
    //{
    //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    //}
    public IActionResult Error(int code)
    {
        switch (code)
        {
            case 404:
                return View("NotFound"); // Create a NotFound view for 404 errors
            // You can handle other status codes (like 500) here as well
            default:
                return View("Error"); // A generic error view for other codes
        }
    }
}
