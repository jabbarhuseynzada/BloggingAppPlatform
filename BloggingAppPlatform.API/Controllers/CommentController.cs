using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
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
            var addedComment = _commentService.Add(comment);
            if(addedComment != null)
            {
                return Ok(addedComment.Message);
            }
            else
            {
                return BadRequest("Problem has occured");
            }
        }
    }
}
