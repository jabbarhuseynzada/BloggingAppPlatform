using Business.Abstract;
using Core.Helpers.Security.JWT;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BloggingAppPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost("addPost")]
        public IActionResult AddPost(AddPostDto post)
        {
            var token = Request.Cookies["auth_token"];

            var userId = JwtHelper.GetUserIdFromToken(token);
            post.UserId = userId.Value;
            //post.CoverImageUrl = "bosdu";
            _postService.Add(post);
            if (post != null)
            {
                return Ok("Succesfully added");
            }
            else
            {
                return BadRequest("xeta bas verdi");
            }
        }
        [HttpPost("updatePost")]
        public IActionResult UpdatePost(UpdatePostDto post)
        {
            var token = Request.Cookies["auth_token"];

            var userId = JwtHelper.GetUserIdFromToken(token);

            if (!userId.HasValue)
            {
                return BadRequest("Invalid user ID.");
            }

            var result = _postService.Update(post, userId.Value); // Pass userId to the service

            if (result.Success)
            {
                return Ok("Successfully updated");
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
        [HttpPost("deletePost")]
        public IActionResult DeletePost(int postId)
        {
            // Get the JWT token from the auth_token cookie
            var token = Request.Cookies["auth_token"];

            // Get the userId from the token using the JwtTokenHandler
            var userId = JwtHelper.GetUserIdFromToken(token);

            if (userId.HasValue)
            {
                var result = _postService.Delete(postId, userId.Value);

                if (result.Success)
                {
                    return Ok(result.Message);
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
            else
            {
                // Enhanced error message for debugging
                return BadRequest($"Invalid user ID. UserId: {userId}");
            }
        }


        [HttpGet("getPostsByUserId")]
        public IActionResult GetPostsByUserId(int userId)
        {
            var posts = _postService.GetPostsByUserId(userId);
            if (posts.Success)
                return Ok(posts.Data);
            else
                return BadRequest("Cannot found any posts or you don't have permission");
        }

        [HttpGet("getAllPosts")]
        public IActionResult GetAllPosts()
        {
            var posts = _postService.GetAllPosts();
            if (posts.Success)
                return Ok(posts.Data);
            else
                return BadRequest(posts.Message);
        }
    }
}
