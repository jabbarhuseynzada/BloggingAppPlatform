using AutoMapper;
using BloggingAppPlatform.MVC.Areas.Admin.ViewModels;
using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BloggingAppPlatform.MVC.Areas.Admin.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        public PostsController(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }
        [Area("Admin")]
        public IActionResult Index()
        {
            PostVM vm = new()
            {
                Posts = _postService.GetAllPosts().Data,
            };
            return View(vm);
        }
        [Area("Admin")]
        [HttpPost]
        public IActionResult DeletePost(int PostId)
        {
            var result = _postService.Delete(PostId);
            if (result.Success)
            {
                TempData["Message"] = "User deleted successfully.";
            }
            else
            {
                TempData["Error"] = result.Message;
            }
            return RedirectToAction("Index");
        }
    }
}
