using AutoMapper;
using BloggingAppPlatform.MVC.Areas.Admin.ViewModels;
using Business.Abstract;
using Core.Helpers.Security.JWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloggingAppPlatform.MVC.Areas.Admin.Controllers
{
    [Authorize(Policy = "AdminOrModerator")]
    [Area("Admin")]
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        public PostsController(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            PostVM vm = new()
            {
                Posts = _postService.GetAllPosts().Data,
            };
            return View(vm);
        }
        [HttpPost]
        public IActionResult DeletePost(int postId)
        {
            // Get the JWT token from the auth_token cookie
            var token = Request.Cookies["auth_token"];

            // Get the userId from the token using the JwtHelper
            var userId = JwtHelper.GetUserIdFromToken(token);

            if (!userId.HasValue)
            {
                TempData["Error"] = "Invalid user ID.";
                return RedirectToAction("Index");
            }

            // Pass the userId to the Delete method
            var result = _postService.Delete(postId, userId.Value);

            if (result.Success)
            {
                TempData["Message"] = "Post deleted successfully.";
            }
            else
            {
                TempData["Error"] = result.Message;
            }

            return RedirectToAction("Index");
        }

    }
}
