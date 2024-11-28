using BloggingAppPlatform.MVC.ViewModels;
using Business.Abstract;
using Core.Helpers.Security.JWT;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace BloggingAppPlatform.MVC.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // Handle GET requests
        public IActionResult Index()
        {
            return View();
        }

        // Handle POST request for adding a comment
        [HttpPost]
        public IActionResult AddComment(CommentDto comment)
        {
            var token = Request.Cookies["auth_token"];
            var userId = JwtHelper.GetUserIdFromToken(token);
            comment.UserId = userId.Value;
            if (ModelState.IsValid)
            {
                var result = _commentService.Add(comment);
                if (result.Success)
                {
                    // Redirect to the post page or handle the response
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Return with an error message
                    ModelState.AddModelError("", result.Message);
                }
            }

            // If we got this far, something failed, return the same view with errors
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public IActionResult DeleteComment(int commentId)
        {
            var token = Request.Cookies["auth_token"];
            var userId = JwtHelper.GetUserIdFromToken(token);

            if (!userId.HasValue)
            {
                TempData["Error"] = "Invalid user ID.";
                return RedirectToAction("NotFound", "Home");
            }

            var result = _commentService.Delete(commentId, userId.Value);

            if (result.Success)
            {
                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = result.Message;
            return RedirectToAction("Error", "Home");
        }
        [HttpPost]
        public IActionResult UpdateComment(UpdateCommentDto uComment)
        {
            var token = Request.Cookies["auth_token"];
            var userId = JwtHelper.GetUserIdFromToken(token);

          
            var result = _commentService.Update(uComment, userId.Value);

            if (result.Success)
            {
                TempData["SuccessMessage"] = "Comment updated successfully!";
                return RedirectToAction("Index", "Home");
            }
            else
            {

                ViewBag.ErrorMessage = result.Message;


                UpdateCommentVM vm = new()
                {
                    CommentDto = uComment,
                };
                return View(vm);
            }
        }
        [HttpGet]
        public IActionResult UpdateCommentView(int CommentId, string CommentText)
        {
            UpdateCommentDto updateComment = new()
            {   
                CommentId = CommentId,
                CommentText = CommentText
            };
            UpdateCommentVM vm = new()
            {
                CommentDto = updateComment,
            };
            return View("UpdateComment",vm);

        }
    }
}
