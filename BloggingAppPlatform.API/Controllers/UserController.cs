using Business.Abstract;
using Core.Helpers.Security.JWT;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BloggingAppPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        [HttpPost("addOperationClaim")]
        public IActionResult AddOperationClaimToUser(string username, string operationClaimName)
        {
            var result = _userService.AddOperationClaimToUser(username, operationClaimName);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
        [HttpPost("updateUser")]
        public IActionResult UpdateUser(UpdateUserDto userDto)
        {
            var token = Request.Cookies["auth_token"];

            var userId = JwtHelper.GetUserIdFromToken(token).Value;
            var result = _userService.UpdateUser(userDto, userId);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            else 
            { 
                return BadRequest(result.Message); 
            }
        }

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
