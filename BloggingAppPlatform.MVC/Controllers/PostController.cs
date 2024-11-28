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
        /*[HttpPost]
        public IActionResult UpdatePost(UpdatePostDto post)
        {
            var token = Request.Cookies["auth_token"];

            var userId = JwtHelper.GetUserIdFromToken(token);

            if (!userId.HasValue)
            {
                ViewBag.ErrorMessage = "Invalid user ID.";
                return View(post);
            }
            var result = _postService.Update(post, userId.Value);

            if (result.Success)
            {
                TempData["SuccessMessage"] = "Post updated successfully!";
                return RedirectToAction("Index", "Profile");
            }
            else
            {
                // Set an error message if the update failed
                ViewBag.ErrorMessage = result.Message;
                return View(post); // Return the same view with the error message and the post data
            }
        }*/
        [HttpPost]
        public IActionResult UpdatePost(UpdatePostDto post)
        {
            var token = Request.Cookies["auth_token"];
            var userId = JwtHelper.GetUserIdFromToken(token);

            if (!userId.HasValue)
            {
                ViewBag.ErrorMessage = "Invalid user ID.";
                UpdatePostVm vm = new UpdatePostVm
                {
                    postDto = post 
                };
                return View(vm);
            }

            var result = _postService.Update(post, userId.Value);

            if (result.Success)
            {
                TempData["SuccessMessage"] = "Post updated successfully!";
                return RedirectToAction("Index", "Profile");
            }
            else
            {
                
                ViewBag.ErrorMessage = result.Message;

                
                UpdatePostVm vm = new()
                {
                    postDto = post 
                };
                return View(vm); 
            }
        }

        [HttpGet]
        public IActionResult UpdatePostView(int PostId, string Title, string Context)
        {
            UpdatePostDto _postDto = new()
            {
                PostId = PostId,
                Title = Title,
                Context = Context
            };
            UpdatePostVm vm = new() 
            {
                postDto= _postDto
            };
            return View("UpdatePost", vm);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
