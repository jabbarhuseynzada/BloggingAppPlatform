using BloggingAppPlatform.MVC.ViewModels;
using Business.Abstract;
using Core.Entities.Concrete;
using Core.Helpers.Security.JWT;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BloggingAppPlatform.MVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService; // Assuming you have a UserService to get user details
        private readonly ICommentService _commentService;

        public ProfileController(IPostService postService, IUserService userService, ICommentService commentService)
        {
            _postService = postService;
            _userService = userService;
            _commentService = commentService;
        }

        public IActionResult Index()
        {
            var token = Request.Cookies["auth_token"];
            int userId = JwtHelper.GetUserIdFromToken(token).Value;

            GetUserDto user = _userService.GetUserById(userId);  
            var postsByUser = _postService.GetPostsByUserId(userId).Data;
            var postsCount = postsByUser.Count();
            var commentCount = _commentService.GetCommentsByUserId(userId).Data.Count;  
            PostVM vm = new()
            {
                PostsByUser = postsByUser,
                user = user.User,
                Count = postsCount,
                CommentCount = commentCount,
                FollowerCount= user.FollowerCount
            };

            return View(vm);
        }
        [HttpGet]
        public IActionResult GetProfile(string userName)
        {
            var token = Request.Cookies["auth_token"];
            var userId = JwtHelper.GetUserIdFromToken (token).Value;
            try
            {
                
                User user = _userService.GetByUsername(userName);
                GetUserDto userDto = _userService.GetUserById(user.Id);
                if (user == null)
                {
                    
                    return RedirectToAction("Error", "Home", new { message = "User not found." });
                }

                
                var postsByUser = _postService.GetPostsByUserId(user.Id).Data;
                var postsCount = postsByUser.Count();
                var isFollow = _userService.IsFollow(userId, user.Id).Data;
                
                PostVM vm = new()
                {
                    PostsByUser = postsByUser,
                    user = user,
                    Count = postsCount,
                    IsFollow = isFollow,
                    FollowerCount = userDto.FollowerCount
                };

                return View("UserProfile", vm);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }
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
        [HttpGet]
        public IActionResult UpdateProfileView(int userId, string username, string firstname, string lastname, string email)
        {
            UpdateUserDto updateUserDto = new()
            {
                Id = userId,
                Username = username,
                Firstname = firstname,
                Lastname = lastname,
                Email = email
            };
            UpdateUserVm vm = new()
            {
                User = updateUserDto,
            };
            return View("UpdateProfile", vm);
        }
        [HttpPost]
        public IActionResult UpdateProfile(UpdateUserDto updateUserDto)
        {
            var token = Request.Cookies["auth_token"];
            var userId = JwtHelper.GetUserIdFromToken(token).Value;
            updateUserDto.Id = userId;
            if (ModelState.IsValid)
            {
                var result = _userService.UpdateUser(updateUserDto, userId);
                if (result.Success)
                {
                    return RedirectToAction("Index", "Profile");
                }
                else
                {
                    // Return with an error message
                    ModelState.AddModelError("", result.Message);
                }
            }
            return RedirectToAction("Index", "Profile");
        }
    }
}
