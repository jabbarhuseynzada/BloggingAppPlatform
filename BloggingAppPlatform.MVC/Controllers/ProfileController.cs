using BloggingAppPlatform.MVC.ViewModels;
using Business.Abstract;
using Core.Entities.Concrete;
using Core.Helpers.Security.JWT;
using Microsoft.AspNetCore.Mvc;

namespace BloggingAppPlatform.MVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService; // Assuming you have a UserService to get user details

        public ProfileController(IPostService postService, IUserService userService)
        {
            _postService = postService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            var token = Request.Cookies["auth_token"];
            int userId = JwtHelper.GetUserIdFromToken(token).Value;

            User user = _userService.GetUserById(userId);  
            var postsByUser = _postService.GetPostsByUserId(userId).Data;
            var postsCount = postsByUser.Count();

            PostVM vm = new()
            {
                PostsByUser = postsByUser,
                user = user,
                Count = postsCount
            };

            return View(vm);
        }
        [HttpPost]
        public IActionResult DeletePost(int postId)
        {
            var token = Request.Cookies["auth_token"];
            var userId = JwtHelper.GetUserIdFromToken(token);

            if (!userId.HasValue)
            {
                TempData["Error"] = "Invalid user ID.";
                return RedirectToAction("NotFound", "Home");
            }

            var result = _postService.Delete(postId, userId.Value);

            if (result.Success)
            {
                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = result.Message;
            return RedirectToAction("Error", "Home");
        }

    }
}
