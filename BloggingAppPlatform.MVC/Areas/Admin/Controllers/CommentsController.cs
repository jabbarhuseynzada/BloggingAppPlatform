using BloggingAppPlatform.MVC.Areas.Admin.ViewModels;
using Business.Abstract;
using Core.Helpers.Security.JWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloggingAppPlatform.MVC.Areas.Admin.Controllers
{
    [Authorize(Policy = "AdminOrModerator")]
    [Area("Admin")]
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;
        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        public IActionResult Index()
        {
            CommentVM vm = new() 
            {
                Comments = _commentService.GetAllComments().Data
            };

            return View(vm);
        }
        [HttpPost]
        public IActionResult DeleteComment(int commentId)
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
            var result = _commentService.Delete(commentId, userId.Value);

            if (result.Success)
            {
                TempData["Message"] = "Comment deleted successfully.";
            }
            else
            {
                TempData["Error"] = result.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
