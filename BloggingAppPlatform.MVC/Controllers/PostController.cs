using AutoMapper;
using BloggingAppPlatform.MVC.ViewModels;
using Business.Abstract;
using Core.Helpers.Security.JWT;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloggingAppPlatform.MVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public PostController(IPostService postService, IUserService userService, IMapper mapper)
        {
            _postService = postService;
            _userService = userService;
            _mapper = mapper;
        }
        [HttpPost]
        public IActionResult AddPost(AddPostDto postDto)
        {
            var token = Request.Cookies["auth_token"];
            var userId = JwtHelper.GetUserIdFromToken(token);
            postDto.UserId = userId.Value;
            //postDto.CoverImageUrl = "bosdu";
            _postService.Add(postDto);
             return RedirectToAction("Index", "Post"); // Assuming there's a "PostList" action to list all posts
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
