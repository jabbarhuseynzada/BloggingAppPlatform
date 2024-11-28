using Business.Abstract;
using Core.Helpers.Security.JWT;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BloggingAppPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        [HttpPost("addComment")]
        public IActionResult AddComment(CommentDto comment)
        {
            var token = Request.Cookies["auth_token"];
            var userId = JwtHelper.GetUserIdFromToken(token).Value;
            comment.UserId = userId;
            var addedComment = _commentService.Add(comment);
            if (addedComment != null)
            {
                return Ok(addedComment.Message);
            }
            else
            {
                return BadRequest(addedComment.Message);
            }
        }
        [HttpPost("deleteComment")]
        public IActionResult DeleteComment(int Id)
        {
            var token = Request.Cookies["auth_token"];
            var userId = JwtHelper.GetUserIdFromToken(token).Value;
            var comment = _commentService.Delete(Id, userId);
            if (comment != null)
            {
                return Ok(comment.Message);
            }
            else
            {
                return BadRequest(comment.Message);
            }
        }
        [HttpPost("updateComment")]
        public IActionResult UpdateComment(UpdateCommentDto commentDto)
        {
            var token = Request.Cookies["auth_token"];
            var userId = JwtHelper.GetUserIdFromToken(token).Value;
            var comment = _commentService.Update(commentDto, userId);
            if (comment != null)
            {
                return Ok(comment.Message);
            }
            else
            {
                return BadRequest(comment.Message);
            }
        }

        [HttpGet("getCommentsByUserId")]
        public IActionResult GetCommentsByUserId(int userId)
        {
            var comments = _commentService.GetCommentsByUserId(userId);
            if (comments.Data.Count > 0)
                return Ok(comments.Data);
            else
                return BadRequest(comments.Message);
        }
        [HttpGet("getCommentsByPostId")]
        public IActionResult GetCommentsByPostId(int postId)
        {
            var comments = _commentService.GetCommentsByPostId(postId);
            if (comments.Data.Count > 0)
                return Ok(comments.Data);
            else
                return BadRequest(comments.Message);
        }

    }
}
