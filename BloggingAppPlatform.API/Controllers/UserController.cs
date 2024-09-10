using Business.Abstract;
using Microsoft.AspNetCore.Http;
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
            _userService.AddOperationClaimToUser(userId, claimId);
            return Ok("claim succesfully added");
        }
    }
}
