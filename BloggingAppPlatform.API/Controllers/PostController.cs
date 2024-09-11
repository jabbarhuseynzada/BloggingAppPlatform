using Business.Abstract;
using Core.Helpers.Results.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
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
            _postService.Update(post);
            if(post != null)
                return Ok("Succesfully uptaded");
            else
                return BadRequest("xeta bas verdi");
        }
        [HttpPost("deletePost")]
        public IActionResult DeletePost(int id)
        {
            var post = _postService.Delete(id);
            if (post.Success)
            {
                return Ok(post.Message);
            }else
            {
                return BadRequest(post.Message);
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
