using BloggingAppPlatform.MVC.Areas.Admin.ViewModels;
using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BloggingAppPlatform.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OpClaimsController : Controller
    {
        private readonly IUserService _userService;
        public OpClaimsController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            OpClaimVM opClaimVM = new()
            {
                OperationClaims = _userService.GetAllOperationClaims().Data,
            };
            return View(opClaimVM);
        }
        [HttpPost]
        public IActionResult AddOpClaim(OpClaimDto opClaimDto)
        {
            var result = _userService.AddOperationClaimToUser(opClaimDto.Username, opClaimDto.ClaimName);
            if (result.Success)
            {
                ViewBag.SuccessMessage = result.Message;
                return RedirectToAction("Index", "OpClaims","Admin");
            }
            else
            {
                ViewBag.ErrorMessage = result.Message;
                return RedirectToAction("Index", "OpClaims", "Admin");
            }
        }
    }
}
