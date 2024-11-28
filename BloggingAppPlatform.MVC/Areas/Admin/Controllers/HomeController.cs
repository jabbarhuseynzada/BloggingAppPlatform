using BloggingAppPlatform.MVC.Areas.Admin.ViewModels;
using Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloggingAppPlatform.MVC.Areas.Admin.Controllers
{
    [Authorize(Policy = "AdminOrModerator")]
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        public HomeController(IUserService userService, IPostService postService, ICommentService commentService)
        {
            _userService = userService;
            _postService = postService;
            _commentService = commentService;
        }
        public IActionResult Index()
        {
            HomeVM vm = new()
            {
                UserCount = _userService.GetAllUsers().Data.Count,
                PostCount = _postService.GetAllPosts().Data.Count,
                CommentCount = _commentService.GetAllComments().Data.Count,
            };
            return View(vm);
        }
    }
}
