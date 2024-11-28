using Business.Abstract;
using Core.Helpers.Security.JWT;
using Microsoft.AspNetCore.Mvc;

namespace BloggingAppPlatform.MVC.Controllers
{
    public class FollowController : Controller
    {
        /* public IActionResult Index()
         {
             return View();
         }*/
        private readonly IUserService _userService;
        public FollowController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public IActionResult FollowUser(int followedUserId)
        {
            var token = Request.Cookies["auth_token"];
            var followerId = JwtHelper.GetUserIdFromToken(token!)!.Value;
            var result = _userService.FollowUser(followerId, followedUserId);
            if(result.Success)
            {
                ViewBag.SuccessMesage = result.Message;
                //return RedirectToAction();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = result.Message;
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public IActionResult UnfollowUser(int unfollowedUserId)
        {
            var token = Request.Cookies["auth_token"];
            var followerId = JwtHelper.GetUserIdFromToken(token).Value;
            var result = _userService.UnfollowUser(followerId, unfollowedUserId);
            if (result.Success)
            {
                ViewBag.SuccessMesage = result.Message;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = result.Message;
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
