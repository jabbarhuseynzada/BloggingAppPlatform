using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BloggingAppPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        [HttpPost("addOperationClaim")]
        public IActionResult AddOperationClaimToUser(int userId, int claimId)
        {
            var result = _userService.AddOperationClaimToUser(userId, claimId);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        //[HttpPost("followUser")]
        //public IActionResult FollowUser(int userId)
        //{
        //    var result = _userService.FollowUser(userId);
        //}

        [HttpGet("GetUsers")]
        public IActionResult GetAllUsers()
        {
            var result = _userService.GetAllUsers();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
    }
}
